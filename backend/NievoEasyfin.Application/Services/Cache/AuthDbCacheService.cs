using NievoEasyfin.Application.Data.Context.Cache;
using NievoEasyfin.Application.Data.Cache.Views;
using NievoEasyfin.Application.Data.Entities.Cache;
using System.Text.Json;

namespace NievoEasyfin.Application.Services.Cache
{
    public class AuthDbCacheService : AuthDbCacheContext
    {
        public string PathToken => "User:TokenPasswordReset";

        public AuthDbCacheService() : base()
        {

        }

        public async Task<TokenPasswordResetView?> GetTokenPasswordResetAttempAsync(int userId)
        {
            string? redisData = await Conn.StringGetAsync($"{PathToken}:{userId}");
            if (string.IsNullOrEmpty(redisData))
                return null;

            return JsonSerializer.Deserialize<TokenPasswordResetView>(redisData);
        }

        public async Task<bool> CreateTokenPasswordResetAttempAsync(int userId, string email)
        {
            Random rnd = new Random();

            var tk = new TokenPasswordResetEntity
            {
                UserId = userId,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                PinToken = rnd.Next(1000, 9999)
            };

            var key = $"{PathToken}:{tk.UserId}";
            var serialized = System.Text.Json.JsonSerializer.Serialize(tk);

            await Conn.StringSetAsync(key, serialized, TimeSpan.FromTicks(1200000000));

            return true;
        }
    }
}