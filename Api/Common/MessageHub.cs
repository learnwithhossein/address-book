using System.Threading.Tasks;
using AddressBook.Domain.DTO;
using AddressBook.Service.Common;
using Microsoft.AspNetCore.SignalR;

namespace AddressBook.Api.Common
{
    public class MessageHub : Hub, IMessageHub
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageHub(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendLoginMessageAsync(LoginEventMessageDto message)
        {
            await _hubContext.Clients.All.SendAsync("LoginEven", message);
        }

        public async Task SendImageUploadMessageAsync(ImageUploadEventMessageDto message)
        {
            await _hubContext.Clients.All.SendAsync("ImageUploadEvent", message);
        }
    }
}
