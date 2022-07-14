using GoogleMaps.LocationServices;

namespace ContactsApi.Services;

public class GeoLocationService : IGeoLocationService
{
    private IConfiguration config;
    private GoogleLocationService locationService;

    public GeoLocationService(GoogleLocationService locationService) => this.locationService = locationService;
    
    public GeoPoint GetLatLongFromAddress(GeoAddress address)
    {
        var mapPoint = this.locationService.GetLatLongFromAddress(new AddressData { Address = address.Street, City = address.City, State = address.State });
        return new(mapPoint.Latitude, mapPoint.Longitude);
    }
}

public record GeoPoint(double Latitude, double Longitude);
public record GeoAddress(string Street, string City, string State);
