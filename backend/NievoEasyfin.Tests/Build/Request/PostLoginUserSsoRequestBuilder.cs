using System;
using Bogus;
using NievoEasyfin.Application.Interfaces.Request;

namespace NievoEasyfin.Tests.Build.Request
{
    public class PostLoginUserSsoRequestBuilder : PostLogiPostLoginUserSsoRequest
    {
        private readonly Faker _faker = new Faker("pt_BR");

        /// <summary>
        /// Default setter to build
        /// </summary>
        internal void Default()
        {
            Provider = "google";
            ProviderAccessToken = _faker.Random.AlphaNumeric(100);
        }

        /// <summary>
        /// Setter to build provider
        /// </summary>
        /// <param name="provider">str</param>
        internal void WithProvider(string provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Setter to build provider access token
        /// </summary>
        /// <param name="providerAccessToken">str</param>
        internal void WithProviderAccessToken(string providerAccessToken)
        {
            ProviderAccessToken = providerAccessToken;
        }
    }
}
