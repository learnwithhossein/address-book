﻿using Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Persist
{
    public class Seeder
    {
        public static void Seed(DataContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    FirstName = "Farhan",
                    LastName = "Original"
                };

                userManager.CreateAsync(user, "P@$$w0rd").Wait();
            }

            if (!context.Contacts.Any())
            {
                var reader = new StreamReader("data.json");
                var text = reader.ReadToEnd();
                var contacts = JsonSerializer.Deserialize<IEnumerable<Contact>>(text,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var admin = userManager.FindByEmailAsync("admin@test.com").Result;
                foreach (var contact in contacts)
                {
                    contact.UserId = admin.Id;
                }

                context.Contacts.AddRange(contacts);
                context.SaveChanges();
            }
        }
    }
}
