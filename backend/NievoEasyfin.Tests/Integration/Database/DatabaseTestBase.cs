using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NievoEasyfin.Application.Data.Context.Database;
using NSubstitute;

namespace NievoEasyfin.Tests.Integration.Database;

public abstract class DatabaseTestBase : IDisposable
{
    protected readonly AuthOrigin DbOrigin;
    protected readonly AuthReplica DbReplica;
    private readonly SqliteConnection _connection;

    protected DatabaseTestBase()
    {
        // Cria conexão SQLite em memória
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var optionsOrigin = new DbContextOptionsBuilder<AuthOrigin>()
            .UseSqlite(_connection)
            .Options;

        var optionsReplica = new DbContextOptionsBuilder<AuthReplica>()
            .UseSqlite(_connection)
            .Options;

        // Mock da configuração para satisfazer o construtor do AuthDbContext
        var mockConfig = Substitute.For<IConfiguration>();

        DbOrigin = new AuthOrigin(optionsOrigin, mockConfig);
        DbReplica = new AuthReplica(optionsReplica, mockConfig);

        // Cria o schema do banco
        DbOrigin.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
        DbOrigin.Dispose();
        DbReplica.Dispose();
    }
}
