namespace ContactsApi.Controllers;

public record ContactGetQuery : IRequest<ContactGetResult>
{
    [Required]
    [FromRoute(Name = "id")]
    public string Id { get; init; }
}
