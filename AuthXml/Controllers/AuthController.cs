using AuthXml.DTO;
using AuthXml.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthXml.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")] // Zastosowanie polityki CORS dla całego kontrolera
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            string result = await _authService.LoginAsync(userDto);

            if (result == "Invalid email or password.")
            {
                return Unauthorized(result);
            }
            else
            {
                // Token zwrócony przy pomyślnym logowaniu
                return Ok(new { Token = result });
            }
        }

        [HttpPost("logintojwt")]
        public async Task<IActionResult> LoginToJwt([FromBody] UserDto userDto)
            {
            try
                {
                // Wywołanie logowania z JWT
                var (token, userDetails) = await _authService.LoginWithJwtAsync(userDto);

                // Zwracanie tokenu i szczegółów użytkownika
                return Ok(new
                    {
                    Token = token,
                    UserDetails = userDetails
                    });
                }
            catch (UnauthorizedAccessException ex)
                {
                // Obsługa błędnych danych logowania
                return Unauthorized(new { Message = ex.Message });
                }
            }
        }
}
