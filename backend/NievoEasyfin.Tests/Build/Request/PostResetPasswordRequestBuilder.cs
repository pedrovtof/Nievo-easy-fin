using System;
using Bogus;
using NievoEasyfin.Application.Interfaces.Request;

namespace NievoEasyfin.Tests.Build.Request
{
    public class PostResetPasswordRequestBuilder : PostResetPasswordRequest
    {
        private readonly Faker _faker = new Faker("pt_BR");

        /// <summary>
        /// Default setter to build
        /// </summary>
        internal void Default()
        {
            Email = _faker.Person.Email;
        }

        /// <summary>
        /// Setter to build
        /// </summary>
        /// <param name="email">str</param>
        internal void WithEmail(string email)
        {
            Email = email;
        }
    }
}
