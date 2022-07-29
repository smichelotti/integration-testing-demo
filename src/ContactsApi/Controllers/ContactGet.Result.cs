namespace ContactsApi.Controllers;

public record ContactGetResult(string Id, string FirstName, string LastName, AddressGetResult Address);

//public record AddressGetResult(string Street, string City, string State, string PostalCode, double Latitude, double Longitude);
