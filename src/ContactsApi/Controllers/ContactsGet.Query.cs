namespace ContactsApi.Controllers;

public record ContactsGetQuery : IRequest<List<ContactsGetResult>>;
