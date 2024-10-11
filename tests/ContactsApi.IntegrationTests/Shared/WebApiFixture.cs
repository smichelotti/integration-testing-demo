using ContactsApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using WireMock.Server;

namespace ContactsApi.IntegrationTests.Shared;

public class WebApiFixture : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;
    public WireMockServer GeoLocationStub;

    public async Task InitializeAsync()
    {
        this.GeoLocationStub = WireMockServer.Start(1234);
        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            // Configure all the things
            builder.ConfigureServices((context, services) =>
            {
                // Line below is used for mocking with Moq
                //services.AddTransient<IGeoLocationClient>(x => CreateMockGeoLocation());
            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("MapsDomain", "http://localhost:1234")
                });
            });
        });
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
        this.GeoLocationStub.Stop();
    }

    private static IGeoLocationClient CreateMockGeoLocation()
    {
        var mock = new Mock<IGeoLocationClient>();
        mock.Setup(x => x.GetLatLongFromAddress(It.IsAny<GeoAddress>())).Returns(new GeoPoint(39.5, -76.4));
        return mock.Object;
    }
}