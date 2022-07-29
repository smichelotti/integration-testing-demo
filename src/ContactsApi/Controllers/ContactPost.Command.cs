namespace ContactsApi.Controllers;

public record ContactPostCommand : IRequest<ContactPostResult>
{
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; }
    public AddressPostCommand Address { get; set; }
}

public record AddressPostCommand([Required]string Street, string City, string State, string PostalCode);

