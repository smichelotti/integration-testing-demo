namespace ContactsApi.Controllers;

public class ContactPostHandler : IRequestHandler<ContactPostCommand, ContactPostResult>
{
    private readonly CosmosContext db;
    private readonly IMapper mapper;

    public ContactPostHandler(CosmosContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<ContactPostResult> Handle(ContactPostCommand request, CancellationToken cancellationToken)
    {
        var contact = this.mapper.Map<Contact>(request);
        var savedContact = await this.db.Contacts.InsertItem(contact);
        var response = this.mapper.Map<ContactPostResult>(savedContact);
        return await Task.FromResult(response).ConfigureAwait(false);
    }
}
