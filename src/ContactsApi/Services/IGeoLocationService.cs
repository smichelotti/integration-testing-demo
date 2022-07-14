namespace ContactsApi.Services;

public interface IGeoLocationService
{
    GeoPoint GetLatLongFromAddress(GeoAddress address);
}
