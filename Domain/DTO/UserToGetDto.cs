using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
  public  class UserToGetDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<ContactToGetSimple> Contacts { get; set; }
    }
}
