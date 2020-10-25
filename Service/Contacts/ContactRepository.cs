using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persist;
using Service.Common;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Service.Contacts
{
    public class ContactRepository : GenericRepository<Contact>
    {
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMessageHub _messageHub;

        public ContactRepository(DataContext context, IMapper mapper, IUserAccessor userAccessor,
            IConfiguration configuration, IMessageHub messageHub) : base(context)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _configuration = configuration;
            _messageHub = messageHub;
        }

        public override async Task Update(Contact newEntity)
        {
            var table = Context.Set<Contact>();
            var oldEntity = await table.FindAsync(newEntity.Id);

            oldEntity.Address = newEntity.Address;
            oldEntity.CellNo = newEntity.CellNo;
            oldEntity.Email = newEntity.Email;
            oldEntity.FirstName = newEntity.FirstName;
            oldEntity.LastName = newEntity.LastName;
            oldEntity.TellNo = newEntity.TellNo;
            oldEntity.WorkNo = newEntity.WorkNo;
            oldEntity.ImageUrl = newEntity.ImageUrl;
            oldEntity.WorkAddress = newEntity.WorkAddress;
            oldEntity.JobTitle = newEntity.JobTitle;

            await base.Update(newEntity);
        }

        public async Task<PagedList<ContactToGetDto>> Find(string name, string phone, string address, int pageNumber,
            int pageSize, string orderBy, string sort)
        {
            var user = await _userAccessor.GetUser();

            var table = Context.Contacts.Include(x => x.User)
                .AsQueryable();

            table = table.Where(x => x.UserId == user.Id);

            if (name != null)
            {
                table = table.Where(x =>
                    x.FirstName.ToLower().Contains(name.ToLower()) || x.LastName.ToLower().Contains(name.ToLower()));
            }

            if (phone != null)
            {
                table = table.Where(x => x.CellNo.Contains(phone) || x.TellNo.Contains(phone));
            }

            if (address != null)
            {
                table = table.Where(x => x.Address.ToLower().Contains(address.ToLower()));
            }

            orderBy ??= "Id";
            sort ??= "asc";

            table = sort == "asc" ? table.OrderBy(orderBy) : table.OrderByDescending(orderBy);

            var data = table.Select(x => _mapper.Map<ContactToGetDto>(x))
                .AsQueryable();

            return await PagedList<ContactToGetDto>.Create(data, pageNumber, pageSize);
        }

        public async Task<IEnumerable<ContactToGetDto>> Search(string name)
        {
            var user = await _userAccessor.GetUser();

            var table = Context.Contacts.Include(x => x.User)
                .AsQueryable();

            table = table.Where(x => x.UserId == user.Id);

            if (name != null)
            {
                table = table.Where(x =>
                    x.FirstName.ToLower().Contains(name.ToLower()) || x.LastName.ToLower().Contains(name.ToLower()));
            }

            var data = table.Select(x => _mapper.Map<ContactToGetDto>(x));

            return await data.ToListAsync();
        }

        public async Task<ContactToGetDto> GetDtoById(int id)
        {
            var contact = await Context.Contacts.Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (contact == null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"Contact with Id {id} not found.");
            }

            return _mapper.Map<ContactToGetDto>(contact);
        }

        public async Task<ImageUploadResultDto> UploadImageAsync(int id, IFormFile file)
        {
            var contact = await GetById(id);
            var currentPublicId = contact.PublicId;

            var account = new Account();
            _configuration.Bind("CloudinaryAccount", account);

            var cloudinary = new Cloudinary(account);

            if (file.Length <= 0) throw new RestException(HttpStatusCode.BadRequest, "Invalid file.");

            await using var stream = file.OpenReadStream();

            var param = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Gravity("face").Crop("fill")
            };

            var result = await cloudinary.UploadAsync(param);
            if (result == null || result.Error != null)
            {
                var message = result == null ? "Error while uploading the image." : result.Error.Message;
                throw new RestException(HttpStatusCode.BadRequest, message);
            }

            contact.ImageUrl = result.SecureUrl.AbsoluteUri;
            contact.PublicId = result.PublicId;

            await Context.SaveChangesAsync();

            await _messageHub.SendImageUploadMessageAsync(new ImageUploadEventMessageDto
            {
                ContactId = id
            });

            if (currentPublicId != null)
            {
                var deleteParam = new DeletionParams(currentPublicId);
                await cloudinary.DestroyAsync(deleteParam);
            }

            return new ImageUploadResultDto
            {
                PublicId = contact.PublicId,
                ImageUrl = contact.ImageUrl
            };
        }
    }
}
