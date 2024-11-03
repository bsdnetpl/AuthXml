using AuthXml.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthXml.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateUnixTimestampController : ControllerBase
        {
        private readonly IGenerateUnixTimestampService _generateUnixTimestampService;
        public GenerateUnixTimestampController(IGenerateUnixTimestampService generateUnixTimestampService)
            {
            _generateUnixTimestampService = generateUnixTimestampService;
            }
        [HttpGet ("seconds")]
        public IActionResult GenerateUnixTimestamp()
            {
            return Ok(_generateUnixTimestampService.GetUnixTimestamp());
            }
        [HttpGet ("milliseconds")]
        public IActionResult GenerateUnixTimestampMilliseconds()
            {
            return Ok(_generateUnixTimestampService.GenerateUnixTimestampMilliseconds());
            }
        }
    }
