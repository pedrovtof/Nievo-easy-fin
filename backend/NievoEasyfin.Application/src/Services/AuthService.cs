using Microsoft.AspNetCore.Identity;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Models;
using NievoEasyfin.Application.Data.Views;
using NievoEasyfin.Application.Data.Entities;

namespace NievoEasyfin.Application.Services.Auth
{
    public class AuthService
    {
        private static CryptoPasswordModel _CryptoPassword;

        private static UserModel _userModel;

        public AuthService(CryptoPasswordModel cryptoPassword, UserModel userModel)
        {
            _CryptoPassword = cryptoPassword;
            _userModel = userModel;
        }

        /// <summary>
        /// Method to convert password into hashPass
        /// </summary>
        /// <param name="password">request password</param>
        /// <returns>Hash password</returns>
        public async Task<string> ConvertRequestPasswordToStringAsync(string password)
            => await _CryptoPassword.HashPasswordAsync(password);

        public async Task<UserView> CreateUserAsync(string name, string password, string email)
            => await _userModel.CreateUserAsync(name, password, email);

        public async Task<UserEntity> GetUserByEmailAsync(string email)
            => await _userModel.GetUserByEmailAsync(email);
    }
}