﻿using AuthXml.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AuthXml.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }

        }

    }
