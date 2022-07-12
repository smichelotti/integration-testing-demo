namespace ContactsApi.Controllers;

public class ContactGetHandler : IRequestHandler<ContactGetQuery, ContactGetResult>
{
    private readonly CosmosContext db;
    private readonly IMapper mapper;

    public ContactGetHandler(CosmosContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<ContactGetResult> Handle(ContactGetQuery request, CancellationToken cancellationToken)
    {
        var contact = await this.db.Contacts.Get<Contact>(request.Id, "Contact").ConfigureAwait(false);
        var contactResult = this.mapper.Map<ContactGetResult>(contact);
        return await Task.FromResult(contactResult).ConfigureAwait(false);
    }
}
