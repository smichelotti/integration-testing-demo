namespace ContactsApi.Controllers;

public class ContactPutHandler : IRequestHandler<ContactPutCommand, ContactPutResult>
{
    private readonly CosmosContext db;
    private readonly IMapper mapper;

    public ContactPutHandler(CosmosContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<ContactPutResult> Handle([FromBody]ContactPutCommand command, CancellationToken cancellationToken)
    {
        var contact = this.mapper.Map<Contact>(command.Contact);
        contact.Id = command.Id;
        var savedContact = await this.db.Contacts.UpdateItem(command.Id, "Contact", contact);
        var response = this.mapper.Map<ContactPutResult>(savedContact);
        return await Task.FromResult(response).ConfigureAwait(false);
    }
}
