using ContactsApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ContactsApi.IntegrationTests.Shared;

public class WebApiFixture : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;

    public async Task InitializeAsync()
    {
        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            // Configure all the things
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IGeoLocationService>(x => CreateMockGeoLocation());

            });
        });
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }

    private static IGeoLocationService CreateMockGeoLocation()
    {
        var mock = new Mock<IGeoLocationService>();
        mock.Setup(x => x.GetLatLongFromAddress(It.IsAny<GeoAddress>())).Returns(new GeoPoint(39.5, -76.4));
        return mock.Object;
    }
}