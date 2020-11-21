using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AddressBook.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
