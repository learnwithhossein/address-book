﻿using System.ComponentModel.DataAnnotations;

namespace AddressBook.Domain
{
    public class LoginCredentials
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
