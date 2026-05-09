using WireMock.Server;

namespace NievoEasyfin.Tests.Mocks.Helpers;

public class WireMockFixture : IDisposable
{
    public WireMockServer Server { get; }

    public WireMockFixture()
    {
        // Start WireMock on a fixed port that matches .env
        Server = WireMockServer.Start(8080);
    }

    public void Dispose()
    {
        Server.Stop();
        Server.Dispose();
    }
}

[CollectionDefinition("WireMock collection")]
public class WireMockCollection : ICollectionFixture<WireMockFixture>
{
}
