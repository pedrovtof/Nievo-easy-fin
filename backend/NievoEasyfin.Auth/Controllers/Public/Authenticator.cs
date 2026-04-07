using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Services.Base.Authenticator;
using NievoEasyfin.Application.Interfaces.Response;

namespace NievoEasyfin.Auth.Controllers.Public
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticatorController : Controller
    {
        private static AuthenticatorService _authenticatorService;
        public AuthenticatorController(AuthenticatorService authenticatorService)
        {
            _authenticatorService = authenticatorService;
        }

        [HttpPost("singin")]
        public async Task<IActionResult> PostLoginUserAsync([FromBody] PostLoginUserRequest request)
            => await _authenticatorService.PostLoginUserAsync(request);
    }
}