using AuthXml.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthXml.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")] // Zastosowanie polityki CORS dla całego kontrolera
    public class RSAEncryptorController : ControllerBase
        {
        private readonly IRSAEncryptorService _rsaEncryptorService;

        public RSAEncryptorController(IRSAEncryptorService rsaEncryptorService)
            {
            _rsaEncryptorService = rsaEncryptorService;
            }

        [HttpGet("Encrypt")]
        public IActionResult Encrypt(string text, string timestamp)
            {
            // Szyfrowanie tekstu z użyciem znacznika czasu
            var encryptedText = _rsaEncryptorService.EncryptText(text, "Key/public_Key.pem", timestamp);

            // Zwrócenie zaszyfrowanego tekstu oraz znacznika czasu
            return Ok(new { encryptedText, timestamp });
            }

        [HttpPost("Decrypt")]
        public IActionResult Decrypt(string encryptedText, string timestamp)
            {
            try
                {
                // Deszyfrowanie tekstu z przekazanym znacznikiem czasu
                var decryptedText = _rsaEncryptorService.DecryptText(encryptedText, "Key/private_Key.pem", timestamp);
                return Ok(decryptedText);
                }
            catch (Exception ex)
                {
                // Obsługa błędów
                return BadRequest(new { message = "Błąd podczas deszyfrowania.", error = ex.Message });
                }
            }
        }
    }
