using Domain;
using Microsoft.AspNetCore.Identity;
using Service.Common;
using System.Net;

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

        public LoginResult Login(LoginCredentials loginCredentials)
        {
            var user = _userManager.FindByEmailAsync(loginCredentials.Email).Result;
            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"Email address {loginCredentials.Email} not found.");
            }

            var result = _signInManager.CheckPasswordSignInAsync(user, loginCredentials.Password, false).Result;
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
