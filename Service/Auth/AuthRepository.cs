using System.Linq;
using Domain;
using Microsoft.AspNetCore.Identity;
using Service.Common;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Microsoft.EntityFrameworkCore;
using Persist;
using System;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Service.Auth
{
    public class AuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IMessageHub _messageHub;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager,
            TokenGenerator tokenGenerator,IMessageHub messageHub,DataContext dataContext,IMapper mapper,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _messageHub = messageHub;
            _dataContext = dataContext;
            _mapper = mapper;
           _configuration = configuration;
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
            await _messageHub.SendLoginMessageAsync( new LoginEventMessageDto
            {
                Message =$"{user.FirstName} just logged in.",
                UserId = user.Id
            });

            return new LoginResult
            {
                JwtToken = _tokenGenerator.Generate(user),
                FirstName = user.FirstName
            };
        }
        public async Task<User> Register(UserForRegisterDto userForRegister)
        {
            if (userForRegister.Password != userForRegister.RepeatPassword)
            {
                throw new RestException(HttpStatusCode.BadRequest, $"passwor and repeat is not equal");
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

         var result= await  _userManager.CreateAsync(user,userForRegister.Password);
            if (!result.Succeeded)
            {
                throw new RestException(HttpStatusCode.BadRequest, "There was an error during registeration.");
            }
            SendEmail(user);
            return user;
        }

        private void SendEmail(User user)
        {
            var config = new EmailConfiguration();
            _configuration.Bind("EmailConfiguration", config);
            var fromAddress = new MailAddress(config.From, "From Address Book");
            var toAddress = new MailAddress("maryammollaie.f@gamil.com",$" To {user.FirstName}");
            string fromPassword = config.Password;
            string subject = config.Subject;
            var body = File.ReadAllText(Environment.CurrentDirectory + @"\email-template.html");
            body = body.Replace("FIRST_NAME", user.FirstName).Replace("CONFIRM_URL",$"{config.Url}/{user.Id}");

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
                Body = body
            };
            smtp.Send(message);
        }

        public async Task<UserToGetDto> Get(string id)
        {
            var user = await _dataContext.Users
                .Include(x => x.Contacts)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (user == null) 
            {
                throw new RestException(HttpStatusCode.NotFound, $"User with this id {id} not found.");

            }

            return _mapper.Map<UserToGetDto>(user);
        }

        
    }
}
