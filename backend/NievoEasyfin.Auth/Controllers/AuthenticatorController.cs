using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Services.Auth;
using NievoEasyfin.Application.Interfaces.Response;

namespace NievoEasyfin.Auth.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticatorController : Controller
    {
        private static AuthService _authService;

        public AuthenticatorController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint para autenticar usuários
        /// </summary>
        /// <returns>Retorno de Token JWT</returns>
        [HttpPost("login")]
        public IActionResult PostLoginAsync()
        {
            return Ok("login");
        }
    }
}