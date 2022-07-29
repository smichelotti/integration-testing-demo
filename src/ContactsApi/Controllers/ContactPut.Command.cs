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
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; }
    public AddressPutCommand Address { get; set; }
}
public record AddressPutCommand([Required]string Street, string City, string State, string PostalCode);
