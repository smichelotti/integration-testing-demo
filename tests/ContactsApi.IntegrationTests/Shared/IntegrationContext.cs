using WireMock.Server;

namespace ContactsApi.IntegrationTests.Shared;

[Collection(Fixtures.ScenariosFixture)]
public abstract class IntegrationContext
{
    protected IntegrationContext(WebApiFixture fixture)
    {
        this.Host = fixture.AlbaHost;
        this.GeoLocationStub = fixture.GeoLocationStub;
    }

    public IAlbaHost Host { get; }
    public WireMockServer GeoLocationStub { get; }
}

[CollectionDefinition(Fixtures.ScenariosFixture)]
public class ScenarioCollection : ICollectionFixture<WebApiFixture>
{
}

internal static class Fixtures
{
    public const string ScenariosFixture = "Scenarios Test Collection";
}
