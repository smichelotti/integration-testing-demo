namespace ContactsApi.Controllers;

public record ContactPutCommand : IRequest<ContactPutResult>
{
    [FromRoute(Name = "id")]
    public string Id { get; set; }

    [FromBody]
    public ContactPut Contact { get; set; }
}

public record ContactPut
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AddressPutCommand Address { get; set; }
}
public record AddressPutCommand(string Street, string City, string State, string PostalCode);
