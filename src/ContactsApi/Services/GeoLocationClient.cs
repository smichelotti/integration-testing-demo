using System.Xml.Linq;

namespace ContactsApi.Services;

public class GeoLocationClient : IGeoLocationClient
{
    private IConfiguration config;
    
    public GeoLocationClient(IConfiguration config) => this.config = config;

    public GeoPoint GetLatLongFromAddress(GeoAddress address)
    {
        string url = $"{this.config["MapsDomain"]}/maps/api/geocode/xml?address={address.ToString()}&sensor=false&key={this.config["MapsApiKey"]}";
        XDocument doc = XDocument.Load(url);

        string status = doc.Descendants("status").FirstOrDefault().Value;
        if (status != "OK")
        {
            throw new System.Net.WebException(doc.Descendants("error_message").FirstOrDefault().Value);
        }

        var els = doc.Descendants("result").Descendants("geometry").Descendants("location").FirstOrDefault();
        if (null != els)
        {
            double.TryParse((els.Nodes().First() as XElement).Value, out var latitude);
            double.TryParse((els.Nodes().ElementAt(1) as XElement).Value, out var longitude);
            return new GeoPoint(latitude, longitude);
        }
        return null;
    }
}

public record GeoPoint(double Latitude, double Longitude);

public record GeoAddress(string Street, string City, string State)
{
    public override string ToString() => $"{this.Street}, {this.City}, {this.State}";
}
