using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Auth;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _repository;

        public AuthController(AuthRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// This method is responsible for login the user to the application.
        /// </summary>
        /// <param name="loginCredentials">The user's credentials including an email and password</param>
        /// <returns>If login is successful, it will return a valid JWT token. Otherwise, it will throw a 400/401 error.</returns>
        [HttpPost("login")]
        public IActionResult Login(LoginCredentials loginCredentials)
        {
            return Ok(_repository.Login(loginCredentials));
        }
    }
}
