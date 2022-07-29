namespace ContactsApi.Controllers;

public record ContactPutResult(string Id, string FirstName, string LastName, AddressPutResult Address);

public record AddressPutResult(string Street, string City, string State, string PostalCode, double Latitude, double Longitude);



