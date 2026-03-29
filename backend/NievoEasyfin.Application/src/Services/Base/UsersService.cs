using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Interfaces.Validator;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Models;

namespace NievoEasyfin.Application.Services.Base.Users
{
    public class UsersService : Controller
    {
        private static AuthOrigin _AuthMainNodeDatabase;

        private static AuthReplica _AuthReplicaNodeDatabase;

        private static CryptoPassword _CryptoPassword;

        public UsersService(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase, CryptoPassword cryptoPassword)
        {
            _AuthMainNodeDatabase = authMainNodeDatabase;
            _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
            _CryptoPassword = cryptoPassword;
        }

        public async Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request)
        {
            var validationResult = await new PostCreateUserValidator().ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                ResponseApiError error = new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                return BadRequest(error);
            }

            string hash = _CryptoPassword.HashPassword(request.Password);

            // TODO: Configurar entidade
            // TODO: Se não existir usuário registrar

            return StatusCode(
                201,
                new ResponseApiSucess(new
                {
                    Name = request.Name,
                    Email = request.Email
                })
            );
        }
    }
}