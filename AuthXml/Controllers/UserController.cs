using AuthXml.DB;
using AuthXml.DTO;
using AuthXml.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthXml.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ConnectToDB _context;


        public UserController(UserService userService, ConnectToDB context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] UserAddDto userDto)
            {
            // Walidacja podstawowa (synchroniczna)
            var validator = new UserDtoValidator();
            var validationResult = validator.Validate(userDto);

            if (!validationResult.IsValid)
                {
                return BadRequest(validationResult.Errors);
                }

            // Asynchroniczne sprawdzenie unikalności emaila
            bool isEmailUnique = !await _context.Users.AnyAsync(u => u.Email == userDto.Email);
            if (!isEmailUnique)
                {
                return BadRequest(new { Email = "Email must be unique." });
                }

            // Dodanie użytkownika do bazy danych
            string result = await _userService.AddUserAsync(userDto);
            return Ok(result);
            }
        }
}
