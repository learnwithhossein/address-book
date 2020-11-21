using System.Threading.Tasks;
using AddressBook.Domain;
using AddressBook.Domain.DTO;
using AddressBook.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers
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
        public async Task<IActionResult> LoginAsync(LoginCredentials loginCredentials)
        {
            return Ok(await _repository.LoginAsync(loginCredentials));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto user)
        {
            return Ok(await _repository.Register(user));
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(UserForConfirmationDto user)
        {
            return Ok(await _repository.Confirm(user));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _repository.Get(id));
        }

        [HttpPost("{id}/image")]
        [Authorize]
        public async Task<IActionResult> UploadImage(string id, [FromForm] IFormFile file)
        {
            return Ok(await _repository.UploadImageAsync(id, file));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(User user)
        {
            await _repository.Update(user);
            return Ok();
        }
    }
}
