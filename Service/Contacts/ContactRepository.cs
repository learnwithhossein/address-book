using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;
using Persist;
using Service.Common;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Service.Contacts
{
    public class ContactRepository : GenericRepository<Contact>
    {
        private readonly IConfiguration _configuration;
        private readonly IMessageHub _messageHub;
        private readonly IMapper _mapper;

        public ContactRepository(DataContext context ,IConfiguration configuration,IMessageHub messageHub,IMapper mapper) : base(context)
        {
            _configuration = configuration;
            _messageHub = messageHub;
            _mapper = mapper;
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
            oldEntity.ImageUrl = newEntity.ImageUrl;
            oldEntity.WorkNo = newEntity.WorkNo;
            oldEntity.JobTitle = newEntity.JobTitle;
            oldEntity.WorkAddress = newEntity.WorkAddress;




            await base.Update(newEntity);
        }

        public async Task<PagedList<Contact>> Find(string name, string phone, string address, int pageNumber,
            int pageSize, string orderBy , string sort)
        {
            var table = Context.Set<Contact>()
                .AsQueryable();

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

          

            return await PagedList<Contact>.Create(table, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Contact>> Search(string name)
        {
            var table = Context.Set<Contact>()
                .AsQueryable();

            if (name != null)
            {
                table = table.Where(x =>
                    x.FirstName.ToLower().Contains(name.ToLower()) || x.LastName.ToLower().Contains(name.ToLower()));
            }

            return await table.ToListAsync();
        }

        public async Task<ImageUploadResultDto> UploadImageAsync(int id, IFormFile file)
        {
            var contact = await GetById(id);
            var currentPublicId = contact.PublicId;
            var account = new Account();
            _configuration.Bind("CloudinaryAccount",account);
           var cloudinary = new Cloudinary(account);
           if (file.Length <= 0) throw new RestException(HttpStatusCode.BadRequest, "Invalid File");
           await using var stream = file.OpenReadStream();
           var param = new ImageUploadParams
           {
               File = new FileDescription(file.FileName,stream),
               Transformation = new Transformation().Height(500).Width(500).Gravity("face").Crop("fill")
           };
           var result = await cloudinary.UploadAsync(param);
           if (result==null || result.Error != null)
           {
               var message = result == null ? "Error while uploading image :)" : result.Error.Message;
               throw new RestException(HttpStatusCode.BadRequest, message);
           }

           contact.ImageUrl = result.SecureUrl.AbsoluteUri; 
            contact.PublicId = result.PublicId;
           await Context.SaveChangesAsync();
          await _messageHub.SendImageUploadMessageAsync(new ImageUploadEventMessageDto
           {
              ContactId = id
           });
           
           if (currentPublicId!=null)
           {
               var deleteparam = new DeletionParams(currentPublicId);
               await cloudinary.DestroyAsync(deleteparam);
           }

           return new ImageUploadResultDto  
           {
               PublicId = contact.PublicId,
               ImageUrl = contact.ImageUrl
           };
        }

        public  async Task<ContactToGetDto> GetDtoById(int id)
        {
            var contact = await Context.Contacts
                .Include(x=>x.User)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (contact==null)
            {
                throw new RestException(HttpStatusCode.NotFound,$"Contact with this Id {id} not found.");
            }

            return _mapper.Map<ContactToGetDto>(contact);

        }
    }
}
