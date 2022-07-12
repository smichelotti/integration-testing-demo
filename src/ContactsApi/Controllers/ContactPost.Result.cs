namespace ContactsApi.Controllers;

public record ContactPostResult(string Id, string FirstName, string LastName, AddressPostResult Address);

public record AddressPostResult(string Street, string City, string State, string PostalCode, double Latitude, double Longitude);


