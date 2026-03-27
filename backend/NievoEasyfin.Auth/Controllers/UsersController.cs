using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Services.Auth;
using NievoEasyfin.Application.Interfaces.Response;

namespace NievoEasyfin.Auth.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private static AuthService _authService;

        public UsersController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoints para registrar usuários
        /// </summary>
        /// <param name="Name">Nome</param>
        /// <param name="Password">Senha</param>
        /// <param name="Email">Email</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/Users
        ///     {
        ///        "Name": "John Doe",
        ///        "Password": "password",
        ///        "Email": "John.Doe@example.com"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="409">Usuário já cadastrado</response>
        /// <returns>Confirmação de usuário criado / Erro</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostUserAsync([FromBody] PostUserRequest request)
            => await _authService.PostUserAsync(request);
    }
}