using AuthXml.DB;
using AuthXml.DTO;
using Microsoft.EntityFrameworkCore;

namespace AuthXml.Service
{
    public class AuthService : IAuthService
    {
        private readonly ConnectToDB _context;
        private readonly IGenerateTokenService _tokenService;

        public AuthService(ConnectToDB context, IGenerateTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<string> LoginAsync(UserDto userDto)
        {
            // Wyszukanie użytkownika na podstawie emaila
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null)
            {
                // Użytkownik o podanym emailu nie istnieje
                return "Invalid email or password.";
            }

            // Weryfikacja hasła
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);

            if (isPasswordValid)
            {
                // Generowanie tokenu przy pomyślnym logowaniu
                string token = _tokenService.GenerateToken();
                return token;
            }
            else
            {
                return "Invalid email or password.";
            }
        }
    }
}
