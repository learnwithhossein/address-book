using Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Persist
{
    public class Seeder
    {
        public static void Seed(DataContext context)
        {
            var reader = new StreamReader("data.json");
            var text = reader.ReadToEnd();
            var contacts = JsonSerializer.Deserialize<IEnumerable<Contact>>(text,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!context.Contacts.Any())
            {
                context.Contacts.AddRange(contacts);
                context.SaveChanges();
            }
        }
    }
}
