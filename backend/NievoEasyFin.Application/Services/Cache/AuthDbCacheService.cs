using NievoEasyFin.Application.Data.Context.Cache;
using NievoEasyFin.Application.Data.Cache.Views;
using NievoEasyFin.Application.Data.Entities.Cache;
using System.Text.Json;

namespace NievoEasyFin.Application.Services.Cache
{
    public class AuthDbCacheService : AuthDbCacheContext
    {
        private const string PATH_TOKEN_RESET_PASSWORD = "User:TokenPasswordReset";

        private const string PATH_TOKEN_SINGUP_USER = "User:SingupUser";

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
            string? redisData = await Conn.StringGetAsync($"{PATH_TOKEN_RESET_PASSWORD}:{userId}");
            if (string.IsNullOrEmpty(redisData))
                return null;

            return JsonSerializer.Deserialize<TokenPasswordResetView>(redisData);
        }

        /// <summary>
        /// Method to Get Token validate email by user email
        /// </summary>
        /// <param name="userId">int</param>
        /// <returns>Null/Value from redis</returns>
        public async Task<TokenEmailValidateView?> GetTokenEmailValidateAsync(string email)
        {
            string? redisData = await Conn.StringGetAsync($"{PATH_TOKEN_SINGUP_USER}:{email}");
            if (string.IsNullOrEmpty(redisData))
                return null;

            return JsonSerializer.Deserialize<TokenEmailValidateView>(redisData);
        }

        /// <summary>
        /// Method to create Token password in redis
        /// </summary>
        /// <param name="userId">int</param>
        /// <param name="email">string</param>
        /// <returns>True</returns>
        public async Task<TokenPasswordResetEntity> CreateTokenPasswordResetAttempAsync(int userId, string email)
        {
            var tk = new TokenPasswordResetEntity
            {
                UserId = userId,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                PinToken = rnd.Next(100000, 999999)
            };

            var key = $"{PATH_TOKEN_RESET_PASSWORD}:{tk.UserId}";
            var serialized = JsonSerializer.Serialize(tk);
            await Conn.StringSetAsync(key, serialized, TimeSpan.FromSeconds(CACHE_DATABASE_DEFAULT_TTL));

            return tk;
        }


        /// <summary>
        /// Method to create Token singup in redis
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <param name="name">name of the user</param>
        /// <returns>TokenSingupUserEntity</returns>
        public async Task<TokenSingupUserEntity> CreateTokenSingupUser(string email, string name)
        {
            var tk = new TokenSingupUserEntity
            {
                Email = email,
                Name = name,
                CreatedAt = DateTime.UtcNow,
                PinToken = rnd.Next(100000, 999999)
            };

            var key = $"{PATH_TOKEN_SINGUP_USER}:{tk.Email}";
            var serialized = JsonSerializer.Serialize(tk);
            await Conn.StringSetAsync(key, serialized, TimeSpan.FromSeconds(CACHE_DATABASE_DEFAULT_TTL));

            return tk;
        }

        /// <summary>
        /// Method to validate token
        /// </summary>
        /// <param name="requestToken">Request body token</param>
        /// <param name="cachetoken">Cache Token</param>
        /// <returns>true / false</returns>
        public async Task<bool> ValidateTokenAsync(int requestToken, int cachetoken)
            => requestToken == cachetoken ? true : false;


        /// <summary>
        /// Method to check redis
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<string?> TestCacheServiceAsync(string key, string value)
        {
            key = $"healthcheck:{key}";
            await Conn.StringSetAsync(key, value, TimeSpan.FromSeconds(CACHE_DATABASE_DEFAULT_TTL));
            return await Conn.StringGetAsync(key);
        }
    }
}
