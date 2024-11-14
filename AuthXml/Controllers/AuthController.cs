using AuthXml.DTO;
using AuthXml.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthXml.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
