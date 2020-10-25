using Domain.DTO;
using Microsoft.AspNetCore.SignalR;
using Service.Common;
using System.Threading.Tasks;

namespace Api.Common
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
