using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Service.Common
{
   public interface IMessageHub
   {
       Task SendLoginMessageAsync(LoginEventMessageDto message);
       Task SendImageUploadMessageAsync(ImageUploadEventMessageDto message);
   }
}
