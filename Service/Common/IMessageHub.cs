using System.Threading.Tasks;
using AddressBook.Domain.DTO;

namespace AddressBook.Service.Common
{
    public interface IMessageHub
    {
        Task SendLoginMessageAsync(LoginEventMessageDto message);
        Task SendImageUploadMessageAsync(ImageUploadEventMessageDto message);
    }
}
