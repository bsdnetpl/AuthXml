using AuthXml.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthXml.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class RSAEncryptorController : ControllerBase
        {
        private readonly IRSAEncryptorService _rsaEncryptorService;

        public RSAEncryptorController(IRSAEncryptorService rsaEncryptorService)
            {
            _rsaEncryptorService = rsaEncryptorService;
            }
        [HttpGet]
        public IActionResult Encrypt(string text)
            {
            return Ok(_rsaEncryptorService.EncryptText(text, "Key/public_Key.pem"));
            }
        [HttpPost]
        public IActionResult Decrypt(string encryptedText)
            {
            return Ok(_rsaEncryptorService.DecryptText(encryptedText, "Key/private_Key.pem"));
            }
        }
    }
