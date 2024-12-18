﻿using AuthXml.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthXml.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")] // Zastosowanie polityki CORS dla całego kontrolera
    public class GenerateTokenController : ControllerBase
        {
        private readonly IGenerateTokenService _generateTokenService;

        public GenerateTokenController(IGenerateTokenService generateTokenService)
            {
            _generateTokenService = generateTokenService;
            }

        [HttpPost ("GenerateToken")]
        public IActionResult GenerateToken()
            {
            return Ok(_generateTokenService.GenerateToken(64));
            }
        }
    }
