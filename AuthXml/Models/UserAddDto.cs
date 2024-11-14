using AuthXml.DB;
using AuthXml.DTO;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AuthXml.Models
{
    public class UserAddDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserDtoValidator : AbstractValidator<UserAddDto>
    {
        private readonly ConnectToDB _context;

        public UserDtoValidator(ConnectToDB context)
        {
            _context = context;

            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }

        // Metoda asynchroniczna sprawdzająca unikalność emaila
        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }

}
