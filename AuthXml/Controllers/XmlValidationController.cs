using AuthXml.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthXml.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class XmlValidationController : ControllerBase
        {
        private readonly IXmlValidationService _xmlValidationService;

        public XmlValidationController(IXmlValidationService xmlValidationService)
            {
            _xmlValidationService = xmlValidationService;
            }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateXml(IFormFile file)
            {
            if (file == null || file.Length == 0)
                {
                return BadRequest("Nie przesłano pliku XML.");
                }

            using var stream = file.OpenReadStream();
            var (isValid, errors, data) = await _xmlValidationService.ValidateAndProcessXmlAsync(stream);

            if (!isValid)
                {
                return BadRequest($"Błędy walidacji:\n{errors}");
                }

            return Ok(new { Message = "Plik XML jest poprawny", Data = data });
            }
        }
    }
