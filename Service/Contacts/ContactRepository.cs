using System.Collections.Generic;
using Domain;
using Persist;
using Service.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Contacts
{
    public class ContactRepository : GenericRepository<Contact>
    {
        public ContactRepository(DataContext context) : base(context)
        {
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

            await base.Update(newEntity);
        }

        public async Task<PagedList<Contact>> Find(string name, string phone, string address, int pageNumber,
            int pageSize, string orderBy, string sort)
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
    }
}
