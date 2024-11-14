using AuthXml.Models;
using AuthXml.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthXml.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] UserAddDto userDto)
        {
            // Sprawdzenie poprawności modelu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Dodanie użytkownika do bazy danych
            string result = await _userService.AddUserAsync(userDto);
            return Ok(result);
        }
    }
}
