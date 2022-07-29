namespace ContactsApi.Controllers;

public record ContactDeleteCommand : IRequest
{
    [Required]
    [FromRoute(Name = "id")]
    public string Id { get; init; }
}
