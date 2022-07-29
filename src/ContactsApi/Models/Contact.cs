namespace ContactsApi.Models;

public record Contact(string FirstName, string LastName, Address Address)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DocType { get; } = "Contact";
}

public record Address(string Street, string City, string State, string PostalCode)
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
