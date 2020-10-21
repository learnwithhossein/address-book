using System.Collections.Generic;

namespace Domain.DTO
{
    public class UserToGetDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<ContactToGetSimpleDto> Contacts { get; set; }
    }
}
