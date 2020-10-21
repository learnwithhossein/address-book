using Api.Common;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persist;
using Service.Auth;
using Service.Common;
using Service.Contacts;
using System.Reflection;

namespace Api
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

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
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
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
