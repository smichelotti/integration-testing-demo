namespace ContactsApi.Services;

public interface IGeoLocationClient
{
    GeoPoint GetLatLongFromAddress(GeoAddress address);
}
