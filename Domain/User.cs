using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
