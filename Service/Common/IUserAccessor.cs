using System.Threading.Tasks;
using AddressBook.Domain;

namespace AddressBook.Service.Common
{
    public interface IUserAccessor
    {
        Task<User> GetUser();
    }
}
