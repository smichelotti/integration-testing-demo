namespace ContactsApi.Controllers;

public record ContactPostCommand : IRequest<ContactPostResult>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AddressPostCommand Address { get; set; }
}

public record AddressPostCommand(string Street, string City, string State, string PostalCode);

