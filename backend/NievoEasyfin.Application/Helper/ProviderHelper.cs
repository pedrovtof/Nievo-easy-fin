using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Models;
using Newtonsoft.Json.Linq;
using NievoEasyfin.Application.Interfaces.Response;
using System.Text.Json;

namespace NievoEasyfin.Application.Helper
{
    /// <summary>
    /// Class for validate provider
    /// </summary>
    public class ProviderHelper
    {

        private const string GOOGLE_API = "https://www.googleapis.com/oauth2/v3/userinfo";

        public ProviderHelper()
        {

        }

        /// <summary>
        /// Method to switch between provider types
        /// </summary>
        /// <param name="provider">SsoProviderEntity</param>
        /// <param name="token">token from sso</param>
        /// <returns>api response</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseProvider> ValidateProviderAsync(SsoProviderEntity provider, string? token = null)
        {
            if (token == null)
                throw new ArgumentException("The token for ValidateProviderAsync is necessary");

            ResponseProvider result;

            switch (provider.Name)
            {
                case "google":
                    result = await ProviderGoogleAsync(provider, token);
                    break;

                case "github":
                    result = await ProviderGithub(provider, token);
                    break;

                default:
                    throw new NotImplementedException($"Method Provider->{provider.Name}<-Async didn't implement");
            }

            return result;
        }

        /// <summary>
        /// Method to validate token in google
        /// </summary>
        /// <param name="provider">SsoProviderEntity</param>
        /// <param name="token">Token from sso</param>
        /// <returns>response from api</returns>
        private async Task<ResponseProvider> ProviderGoogleAsync(SsoProviderEntity provider, string token)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(GOOGLE_API);

            if (!response.IsSuccessStatusCode)
            {
                var error = new GoogleResponse();
                error.WithError("Response was invalid from google");
                return error;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<GoogleResponse>(responseContent);
        }

        /// <summary>
        /// Method to validate token in github
        /// </summary>
        /// <param name="provider">SsoProviderEntity</param>
        /// <param name="token">Token from sso</param>
        /// <returns>response from api</returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<ResponseProvider> ProviderGithub(SsoProviderEntity provider, string? token = null)
        {
            throw new NotImplementedException("Method not yeat implemented");
        }

    }
}