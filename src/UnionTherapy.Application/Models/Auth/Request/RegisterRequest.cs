﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Models.Auth.Request
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
