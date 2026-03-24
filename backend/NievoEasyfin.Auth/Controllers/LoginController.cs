using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Services.Auth;

namespace NievoEasyfin.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class LoginController : ControllerBase
    {
        private static AuthService _authService;

        public LoginController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult PostLoginAsync()
        {
            return Ok("login");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("user")]
        public async Task<IActionResult> PostUserAsync([FromBody] PostUserRequest request)
            => await _authService.PostUserAsync(request);
    }
}