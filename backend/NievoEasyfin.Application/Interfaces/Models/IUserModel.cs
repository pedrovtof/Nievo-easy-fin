using NievoEasyfin.Application.Data.Entities;

namespace NievoEasyfin.Application.Interfaces.Models;

public interface IUserModel
{
    Task<UserEntity> CreateUserAsync(string name, string password, string email, int statusId = 1, int? phone = null);
    Task<UserEntity> CreateUserSsoAsync(string name, string email, string sub, int statusId = 1, int? phone = null);
    Task<UserEntity> GetUserByEmailAsync(string email, int statusId = 1);
    Task<UserEntity> GetUserByEmailWithAnyStatusAsync(string email);
    Task<UserEntity> GetUserByProviderSubAndIdAsync(string subId, int providerId);
    Task<bool> UpdateUserPasswordAsync(int userId, string password);
}
