using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Interfaces.Validator;

namespace NievoEasyfin.Application.Services.Base.Authenticator
{
    public class AuthenticatorService : Controller
    {
        private static AuthOrigin _AuthMainNodeDatabase;

        private static AuthReplica _AuthReplicaNodeDatabase;

        public AuthenticatorService(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
        {
            _AuthMainNodeDatabase = authMainNodeDatabase;
            _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
        }

        public async Task<IActionResult> PostLoginUserAsync(PostLoginUserRequest request)
        {
            var validatorResult = await new PostLoginUserValidatorAsync().ValidateAsync(request);
            if (!validatorResult.IsValid)
                return BadRequest(
                    new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
                );

            return null;
        }
    }
}
