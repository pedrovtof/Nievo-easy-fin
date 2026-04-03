using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Data.Context.Database;

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
    }
}