using System.Reflection;
using AddressBook.Api.Common;
using AddressBook.Persist;
using AddressBook.Service.Auth;
using AddressBook.Service.Common;
using AddressBook.Service.Contacts;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("SqliteConnection"));
                //options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"));
            });

            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            services.AddScoped<ContactRepository, ContactRepository>();
            services.AddScoped<AuthRepository, AuthRepository>();
            services.AddScoped<TokenGenerator, TokenGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IMessageHub, MessageHub>();

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddSecurity(Configuration);

            services.AddControllers();

            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseUnhandledExceptionHandler();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/message");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
