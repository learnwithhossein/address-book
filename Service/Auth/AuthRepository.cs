using AutoMapper;
using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persist;
using Service.Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Service.Auth
{
    public class AuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager,
            TokenGenerator tokenGenerator, IConfiguration configuration, DataContext dataContext, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _dataContext = dataContext;
           _mapper = mapper;
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

            if (!user.EmailConfirmed)
            {
                throw new RestException(HttpStatusCode.BadRequest, "Your account is not confirmed yet.");
            }

            return new LoginResult
            {
                JwtToken = _tokenGenerator.Generate(user),
                FirstName = user.FirstName
            };
        }

        public async Task<UserToGetDto> Get(string id)
        {
            var user = await _dataContext.Users.Include(x => x.Contacts).SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"User Id {id} not found.");
            }

            return _mapper.Map<UserToGetDto>(user);
        }

        public async Task<UserToReturnInConfirmationDto> Confirm(UserForConfirmationDto userForConfirmation)
        {
            var user = await _userManager.FindByIdAsync(userForConfirmation.Id);
            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, $"User Id {userForConfirmation.Id} not found.");
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return new UserToReturnInConfirmationDto
            {
                FirstName = user.FirstName,
                Email = user.Email
            };
        }

        public async Task<User> Register(UserForRegisterDto userForRegister)
        {
            if (userForRegister.Password != userForRegister.RepeatPassword)
            {
                throw new RestException(HttpStatusCode.BadRequest, $"Password and repeat are not equal.");
            }

            var user = await _userManager.FindByEmailAsync(userForRegister.Email);
            if (user != null)
            {
                throw new RestException(HttpStatusCode.BadRequest, $"Email address {userForRegister.Email} already exists.");
            }

            user = new User
            {
                UserName = userForRegister.Email,
                Email = userForRegister.Email,
                FirstName = userForRegister.FirstName,
                LastName = userForRegister.LastName
            };

            var result = await _userManager.CreateAsync(user, userForRegister.Password);
            if (!result.Succeeded)
            {
                var error = string.Join("<br>", result.Errors.Select(x => x.Description));

                throw new RestException(HttpStatusCode.BadRequest, error);
            }

            SendEmail(user);

            return user;
        }

        private void SendEmail(User user)
        {
            var config = new EmailConfiguration();
            _configuration.Bind("EmailConfiguration", config);

            var fromAddress = new MailAddress(config.From, "From Address Book");
            var toAddress = new MailAddress(user.Email, $"To {user.FirstName}");
            var fromPassword = config.Password;
            var subject = config.Subject;

            var body = File.ReadAllText(Environment.CurrentDirectory + @"\email-template.html");
            body = body.Replace("FIRST_NAME", user.FirstName).Replace("CONFIRM_URL", $"{config.Url}/{user.Id}");

            var smtp = new SmtpClient
            {
                Host = config.Host,
                Port = config.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }
    }
}
