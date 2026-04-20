using StackExchange.Redis;
namespace NievoEasyfin.Application.Data.Context.Cache;

/// <summary>
/// Redis cache database context
/// </summary>
public class AuthDbCacheContext
{
    static readonly string CACHE_DATABASE_HOST = DotNetEnv.Env.GetString("CACHE_DATABASE_HOST");

    static readonly int CACHE_DATABASE_PORT = DotNetEnv.Env.GetInt("CACHE_DATABASE_PORT");

    static readonly int CACHE_DATABASE_NUMBER = DotNetEnv.Env.GetInt("CACHE_DATABASE_NUMBER");

    static readonly int CACHE_DATABASE_TIMEOUT_CONNECT = DotNetEnv.Env.GetInt("CACHE_DATABASE_TIMEOUT_CONNECT");

    static readonly int CACHE_DATABASE_SYNCTIMEOUT_CONNECT = DotNetEnv.Env.GetInt("CACHE_DATABASE_SYNCTIMEOUT_CONNECT");

    static readonly int CACHE_DATABASE_ASYNCTIMEOUT_CONNECT = DotNetEnv.Env.GetInt("CACHE_DATABASE_ASYNCTIMEOUT_CONNECT");

    static readonly int CACHE_DATABASE_CONNECTION_RETRY = DotNetEnv.Env.GetInt("CACHE_DATABASE_CONNECTION_RETRY");

    static readonly int CACHE_DATABASE_RETRY_POLICY_MIN_CONNECT = DotNetEnv.Env.GetInt("CACHE_DATABASE_RETRY_POLICY_MIN_CONNECT");

    static readonly int CACHE_DATABASE_RETRY_POLICY_MAX_CONNECT = DotNetEnv.Env.GetInt("CACHE_DATABASE_RETRY_POLICY_MAX_CONNECT");

    static readonly bool CACHE_DATABASE_ABORT_ON_CONNECT_FAIL = DotNetEnv.Env.GetBool("CACHE_DATABASE_ABORT_ON_CONNECT_FAIL");

    static readonly int CACHE_DATABASE_DEFAULT_TTL = DotNetEnv.Env.GetInt("CACHE_DATABASE_DEFAULT_TTL");

    private IDatabase Conn { get; set; }

    /// <summary>
    /// Default constructor to Cache context
    /// </summary>
    public AuthDbCacheContext()
    {
        Conn = ConnectToCacheSync();
    }

    /// <summary>
    /// Method to configure Cache connection
    /// </summary>
    /// <returns>Context Redis</returns>
    private IDatabase ConnectToCacheSync()
    {
        string connectionString = $"{CACHE_DATABASE_HOST}:{CACHE_DATABASE_PORT}";
        var configOptions = ConfigurationOptions.Parse(connectionString);

        configOptions.ConnectTimeout = CACHE_DATABASE_TIMEOUT_CONNECT;
        configOptions.SyncTimeout = CACHE_DATABASE_SYNCTIMEOUT_CONNECT;
        configOptions.AsyncTimeout = CACHE_DATABASE_ASYNCTIMEOUT_CONNECT;
        configOptions.ConnectRetry = CACHE_DATABASE_CONNECTION_RETRY;
        configOptions.ReconnectRetryPolicy = new ExponentialRetry(CACHE_DATABASE_RETRY_POLICY_MIN_CONNECT, CACHE_DATABASE_RETRY_POLICY_MAX_CONNECT);
        configOptions.AbortOnConnectFail = CACHE_DATABASE_ABORT_ON_CONNECT_FAIL;

        var muxer = ConnectionMultiplexer.Connect(configOptions);
        return muxer.GetDatabase(CACHE_DATABASE_NUMBER);
    }

    /// <summary>
    /// Method to test connection
    /// </summary>
    /// <returns>String bar</returns>
    public async Task<string?> TesteConnectionCacheAsync()
    {
        await Conn.StringSetAsync("foo", "bar", TimeSpan.FromTicks(CACHE_DATABASE_DEFAULT_TTL));
        string? fooResult = await Conn.StringGetAsync("foo");
        return fooResult;
    }
}