using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using Microsoft.AspNetCore.SignalR;
using Service.Common;

namespace Api.Controllers
{
    public class MessageHub : Hub,IMessageHub
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageHub(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendLoginMessageAsync(LoginEventMessageDto message)
        {
         await _hubContext.Clients.All.SendAsync("LoginEvent", message);

        }

        public async Task SendImageUploadMessageAsync(ImageUploadEventMessageDto message)
        {
            await _hubContext.Clients.All.SendAsync("ImageUploadEvent", message);

        }
    }
}
