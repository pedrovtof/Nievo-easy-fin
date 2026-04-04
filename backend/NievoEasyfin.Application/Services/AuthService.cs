using Microsoft.AspNetCore.Identity;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Models;
using NievoEasyfin.Application.Data.Views;
using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Helper;

namespace NievoEasyfin.Application.Services.Auth
{
    public class AuthService
    {
        private static CryptoPasswordModel _cryptoPassword;

        private static UserModel _userModel;

        private static SsoProviderModel _ssoProvider;

        public AuthService(CryptoPasswordModel cryptoPassword, UserModel userModel, SsoProviderModel ssoProvider)
        {
            _cryptoPassword = cryptoPassword;
            _userModel = userModel;
            _ssoProvider = ssoProvider;
        }

        /// <summary>
        /// Method to convert password into hashPass
        /// </summary>
        /// <param name="password">request password</param>
        /// <returns>Hash password</returns>
        public async Task<string> ConvertRequestPasswordToStringAsync(string password)
            => await _cryptoPassword.HashPasswordAsync(password);

        #region User

        /// <summary>
        /// Create user entity
        /// </summary>
        /// <param name="name">request.name</param>
        /// <param name="password">request.password</param>
        /// <param name="email">request.email</param>
        /// <returns>userView</returns>
        public async Task<UserView> CreateUserAsync(string name, string password, string email)
            => await _userModel.CreateUserAsync(name, password, email);

        /// <summary>
        /// Create user entity
        /// </summary>
        /// <param name="name">provider.response.name</param>
        /// <param name="email">provider.response.password</param>
        /// <param name="sub">provider.response.email</param>
        /// <returns>userView</returns>
        public async Task<UserView> CreateUserSsoAsync(string name, string email, string sub)
            => await _userModel.CreateUserSsoAsync(name, email, sub);

        /// <summary>
        /// Search user by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>UserEntity</returns>
        public async Task<UserEntity> GetUserByEmailAsync(string email)
            => await _userModel.GetUserByEmailAsync(email);

        public async Task<UserEntity> GetUserBySubId(string sub)
            => null;

        /// <summary>
        ///  Search provider by name
        /// </summary>
        /// <param name="provider">name of the provider</param>
        /// <returns>SsoProviderEntity</returns>
        public async Task<SsoProviderEntity> GetProviderByNameAsync(string provider)
            => await _ssoProvider.GetProviderByNameAsync(provider);

        /// <summary>
        /// Method to validate the type of provider from sso
        /// </summary>
        /// <param name="provider">SsoProviderEntity</param>
        /// <param name="token">response string token from api sso</param>
        /// <returns></returns>
        public async Task<ResponseProvider> ProviderValidateAsync(string provider, string token)
            => await _ssoProvider.ValidateProviderAsync(provider, token);

        #endregion User
    }
}