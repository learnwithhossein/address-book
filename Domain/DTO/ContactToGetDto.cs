using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO
{
    public class ContactToGetDto
    {

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string TellNo { get; set; }
        public string CellNo { get; set; }
        public string JobTitle { get; set; }
        public string WorkNo { get; set; }
        public string WorkAddress { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public string UserFullName { get; set; }
    }
}
