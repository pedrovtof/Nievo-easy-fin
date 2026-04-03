using Microsoft.AspNetCore.Identity;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Models;
using NievoEasyfin.Application.Data.Views;
using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Models;

namespace NievoEasyfin.Application.Services.Auth
{
    public class AuthService
    {
        private static CryptoPasswordModel _CryptoPassword;

        private static UserModel _userModel;

        private static SsoProviderModel _ssoProvider;

        public AuthService(CryptoPasswordModel cryptoPassword, UserModel userModel, SsoProviderModel ssoProvider)
        {
            _CryptoPassword = cryptoPassword;
            _userModel = userModel;
            _ssoProvider = ssoProvider;
        }

        /// <summary>
        /// Method to convert password into hashPass
        /// </summary>
        /// <param name="password">request password</param>
        /// <returns>Hash password</returns>
        public async Task<string> ConvertRequestPasswordToStringAsync(string password)
            => await _CryptoPassword.HashPasswordAsync(password);

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
        /// Search user by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>UserEntity</returns>
        public async Task<UserEntity> GetUserByEmailAsync(string email)
            => await _userModel.GetUserByEmailAsync(email);

        public async Task<SsoProviderEntity> GetProviderByNameAsync(string provider)
            => await _ssoProvider.GetProviderByNameAsync(provider);
    }
}