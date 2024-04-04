﻿using GeoQuest.Enums;

namespace GeoQuest.DTOs
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserRole Role { get; set; }
    }
}
