namespace ContactsApi.Controllers;

public class ContactDeleteHandler : IRequestHandler<ContactDeleteCommand>
{
    private readonly CosmosContext db;

    public ContactDeleteHandler(CosmosContext db) => this.db = db;

    public async Task<Unit> Handle(ContactDeleteCommand command, CancellationToken cancellationToken)
    {
        await this.db.Contacts.DeleteItem<Contact>(command.Id, "Contact");
        return default;
    }
}
