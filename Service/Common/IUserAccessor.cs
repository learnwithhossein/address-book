using Domain;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface IUserAccessor
    {
        Task<User> GetUser();
    }
}
