using Microsoft.AspNetCore.StaticAssets;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace NievoEasyfin.Application.Data.Context;

public class AuthDbCacheContext
{

    static readonly string CACHE_DATABASE_HOST = DotNetEnv.Env.GetString("CACHE_DATABASE_HOST");

    static readonly int CACHE_DATABASE_PORT = DotNetEnv.Env.GetInt("CACHE_DATABASE_PORT");

    static IDatabase Conn { get; set; }

    public AuthDbCacheContext()
    {
        Conn = ConnectToCacheSync();
    }

    private IDatabase ConnectToCacheSync()
    {
        var muxer = ConnectionMultiplexer.Connect($"{CACHE_DATABASE_HOST}:{CACHE_DATABASE_PORT}");
        return muxer.GetDatabase();
    }

    public async Task<string?> TesteConnectionCacheAsync()
    {
        await Conn.StringSetAsync("foo", "bar");
        string? fooResult = await Conn.StringGetAsync("foo");
        return fooResult;
    }
}