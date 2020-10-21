using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Service.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Common
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;

        public UserAccessor(IHttpContextAccessor accessor, UserManager<User> userManager)
        {
            _accessor = accessor;
            _userManager = userManager;
        }

        public async Task<User> GetUser()
        {
            var claim = _accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id");
            if (claim == null)
            {
                return null;
            }

            return await _userManager.FindByIdAsync(claim.Value);
        }
    }
}
