using System.Collections.Generic;

namespace Domain.DTO
{
    public class UserToGetDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<ContactToGetSimpleDto> Contacts { get; set; }
    }
}
