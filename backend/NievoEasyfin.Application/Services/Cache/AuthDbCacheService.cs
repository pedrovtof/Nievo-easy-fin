using NievoEasyfin.Application.Data.Context.Cache;
using NievoEasyfin.Application.Data.Cache.Views;
using NievoEasyfin.Application.Data.Entities.Cache;
using System.Text.Json;
using NRedisStack.DataTypes;

namespace NievoEasyfin.Application.Services.Cache
{
    public class AuthDbCacheService : AuthDbCacheContext
    {
        public string PathToken => "User:TokenPasswordReset";

        private readonly Random rnd = new Random();

        public AuthDbCacheService() : base()
        {

        }

        /// <summary>
        /// Method to Get Token password by user ID
        /// </summary>
        /// <param name="userId">int</param>
        /// <returns>Null/Value from redis</returns>
        public async Task<TokenPasswordResetView?> GetTokenPasswordResetAttempByUserIdAsync(int userId)
        {
            string? redisData = await Conn.StringGetAsync($"{PathToken}:{userId}");
            if (string.IsNullOrEmpty(redisData))
                return null;

            return JsonSerializer.Deserialize<TokenPasswordResetView>(redisData);
        }

        /// <summary>
        /// Method to create Token password in redis
        /// </summary>
        /// <param name="userId">int</param>
        /// <param name="email">string</param>
        /// <returns>True</returns>
        public async Task<bool> CreateTokenPasswordResetAttempAsync(int userId, string email)
        {
            var tk = new TokenPasswordResetEntity
            {
                UserId = userId,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                PinToken = rnd.Next(100000, 999999)
            };

            var key = $"{PathToken}:{tk.UserId}";
            var serialized = JsonSerializer.Serialize(tk);
            await Conn.StringSetAsync(key, serialized, TimeSpan.FromSeconds(CACHE_DATABASE_DEFAULT_TTL));

            return true;
        }
    }
}