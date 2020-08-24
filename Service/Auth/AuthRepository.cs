using Domain;
using Microsoft.AspNetCore.Identity;
using Service.Common;
using System.Net;
using System.Threading.Tasks;

namespace Service.Auth
{
    public class AuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenGenerator _tokenGenerator;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager,
            TokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResult> LoginAsync(LoginCredentials loginCredentials)
        {
            var user = await _userManager.FindByEmailAsync(loginCredentials.Email);
            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"Email address {loginCredentials.Email} not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginCredentials.Password, false);
            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.BadRequest, "Email/Password is invalid.");
            }

            return new LoginResult
            {
                JwtToken = _tokenGenerator.Generate(user),
                FirstName = user.FirstName
            };
        }
    }
}
