using AuthXml.DB;
using AuthXml.Models;

namespace AuthXml.Service
{
    public class UserService : IUserService
    {
        private readonly ConnectToDB _context;

        public UserService(ConnectToDB context)
        {
            _context = context;
        }

        public async Task<string> AddUserAsync(UserAddDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                DateTimeCreate = DateTime.UtcNow
            };

            // Dodanie użytkownika do kontekstu i zapisanie zmian
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Zwracanie komunikatu po zakończeniu operacji
            return "User added successfully!";
        }
    }
}
