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

        [HttpPost("login")]
        public IActionResult Login(LoginCredentials loginCredentials)
        {
            return Ok(_repository.Login(loginCredentials));
        }
    }
}
