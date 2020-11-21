using System;
using AddressBook.Domain;
using AddressBook.Persist;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace AddressBook.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var configuration = new ConfigurationBuilder()
            //     .AddJsonFile(Environment.CurrentDirectory + @"\appsettings.json")
            //     .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .WriteTo.Console()
                // .WriteTo.MSSqlServer(configuration.GetConnectionString("SqlServerConnection"),
                //     new SinkOptions
                //     {
                //         AutoCreateSqlTable = true,
                //         TableName = "Logs"
                //     }, restrictedToMinimumLevel: LogEventLevel.Error)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            MigrateDatabase(host);

            host.Run();
        }

        private static void MigrateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                context.Database.Migrate();
                Seeder.Seed(context, userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
