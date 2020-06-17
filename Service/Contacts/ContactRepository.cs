using Domain;
using Persist;
using System.Collections.Generic;
using System.Linq;

namespace Service.Contacts
{
    public class ContactRepository : GenericRepository<Contact>
    {
        public ContactRepository(DataContext context) : base(context)
        {
        }

        public override void Update(Contact newEntity)
        {
            var table = Context.Set<Contact>();
            var oldEntity = table.Find(newEntity.Id);

            oldEntity.Address = newEntity.Address;
            oldEntity.CellNo = newEntity.CellNo;
            oldEntity.Email = newEntity.Email;
            oldEntity.FirstName = newEntity.FirstName;
            oldEntity.LastName = newEntity.LastName;
            oldEntity.TellNo = newEntity.TellNo;

            base.Update(newEntity);
        }

        public IEnumerable<Contact> Find(string name, string phone, string address)
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

            return table;
        }
    }
}
