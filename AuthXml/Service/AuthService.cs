using AuthXml.DB;
using AuthXml.DTO;
using AuthXml.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthXml.Service
{
    public class AuthService : IAuthService
        {
        private readonly ConnectToDB _context;
        private readonly IGenerateTokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(ConnectToDB context, IGenerateTokenService tokenService, IConfiguration configuration)
            {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
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
                string token = _tokenService.GenerateToken(64);
                user.Token = token;
                //_context.Users.Update(user);
                await _context.SaveChangesAsync();
                return token;
                }
            else
                {
                return "Invalid email or password.";
                }
            }

        public async Task<(string Token, UserDetailsDto UserDetails)> LoginWithJwtAsync(UserDto userDto)
            {
            // Wyszukanie użytkownika na podstawie emaila
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null)
                {
                throw new UnauthorizedAccessException("Invalid email or password.");
                }

            // Weryfikacja hasła
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);

            if (!isPasswordValid)
                {
                throw new UnauthorizedAccessException("Invalid email or password.");
                }

            // Generowanie tokenu JWT z rolą
            string token = GenerateJwtToken(user);

            // Przygotowanie danych użytkownika do zwrócenia
            var userDetails = new UserDetailsDto
                {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = "user"
                };

            return (token, userDetails);
            }

        private string GenerateJwtToken(User user)
            {
            // Pobranie klucza z konfiguracji
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tworzenie claimów (w tym roli użytkownika)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role ?? "user") // Domyślnie ustaw "user", jeśli brak roli
            };

            // Tworzenie tokenu JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
            }

        }
    }
