using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Services.Base.Users;
using NievoEasyfin.Application.Interfaces.Response;

namespace NievoEasyfin.Auth.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private static UsersService _usersService;

        public UsersController(UsersService users)
        {
            _usersService = users;
        }

        /// <summary>
        /// Endpoints to create user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/Users
        ///     {
        ///        "name": "Joe Black",
        ///        "password": "1Meet-Death",
        ///        "email": "Joe.Black@example.com"
        ///     }
        /// </remarks>
        /// <param name="request">Dados do usuário para registro (Nome, Senha, Email)</param>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Requisição inválida</response>
        /// <response code="409">Usuário já cadastrado</response>
        /// <returns>Confirmação de usuário criado / Erro</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostCreateUserAsync([FromBody] PostCreateUserRequest request)
            => await _usersService.PostCreateUserAsync(request);
    }
}