using AuthXml.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AuthXml.Models
{
    public class User
    {
        int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateTimeCreate { get; set; }

    }

}
