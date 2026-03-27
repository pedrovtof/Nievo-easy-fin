using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Validator;
using NievoEasyfin.Application.Interfaces.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NievoEasyfin.Application.Services.Auth
{
    public class AuthService : ControllerBase
    {
        private static AuthOrigin _AuthMainNodeDatabase;

        private static AuthReplica _AuthReplicaNodeDatabase;

        public AuthService(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
        {
            _AuthMainNodeDatabase = authMainNodeDatabase;
            _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
        }

        public async Task<IActionResult> PostUserAsync(PostUserRequest request)
        {
            var validationResult = await new PostUserValidator().ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                ResponseApiError error = new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                return BadRequest(error);
            }

            object valuesToBeReturn = new
            {
                Name = request.Name,
                Password = request.Password,
                Email = request.Email
            };

            return Ok(new ResponseApiSucess(valuesToBeReturn));
        }
    }
}