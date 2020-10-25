using Domain.DTO;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface IMessageHub
    {
        Task SendLoginMessageAsync(LoginEventMessageDto message);
        Task SendImageUploadMessageAsync(ImageUploadEventMessageDto message);
    }
}
