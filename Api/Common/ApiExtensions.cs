using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AddressBook.Domain;
using AddressBook.Persist;
using AddressBook.Service.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AddressBook.Api.Common
{
    public static class ApiExtensions
    {
        public static IApplicationBuilder UseUnhandledExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    var error = feature.Error;

                    var statusCode = error is RestException restException
                        ? restException.StatusCode
                        : HttpStatusCode.InternalServerError;

                    context.Response.StatusCode = (int)statusCode;
                    var message = statusCode == HttpStatusCode.InternalServerError
                        ? "An error occured in server, please try later."
                        : error.Message;
                    await context.Response.WriteAsync(message);
                });
            });

            return app;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>()
                .AddUserManager<UserManager<User>>()
                .AddSignInManager<SignInManager<User>>()
                .AddEntityFrameworkStores<DataContext>();

            var secret = configuration.GetSection("Secret").Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Query["access_token"];
                            var path = context.Request.Path;

                            if (path.StartsWithSegments("/message") && !string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "This is the documentation of Address Book API's project.",
                    Version = "v1",
                    Title = "API"
                });

                var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var path = Path.Combine(AppContext.BaseDirectory, file);
                options.IncludeXmlComments(path);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please paste your token as follows: Bearer TOKEN",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        public static void AddPagination(this HttpResponse response, Pagination pagination)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(pagination, settings));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
