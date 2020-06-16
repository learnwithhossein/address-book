using Domain;
using Persist;

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
    }
}
