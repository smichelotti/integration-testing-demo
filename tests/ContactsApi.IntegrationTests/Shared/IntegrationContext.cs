namespace ContactsApi.IntegrationTests.Shared;

[Collection(Fixtures.ScenariosFixture)]
public abstract class IntegrationContext
{
    protected IntegrationContext(WebApiFixture fixture)
    {
        this.Host = fixture.AlbaHost;
    }

    public IAlbaHost Host { get; }
}

[CollectionDefinition(Fixtures.ScenariosFixture)]
public class ScenarioCollection : ICollectionFixture<WebApiFixture>
{
}

internal static class Fixtures
{
    public const string ScenariosFixture = "Scenarios Test Collection";
}
