using Microsoft.Azure.Cosmos;

namespace ContactsApi.Controllers;

public class ContactsGetHandler : IRequestHandler<ContactsGetQuery, List<ContactsGetResult>>
{
    private readonly CosmosContext db;
    private readonly IMapper mapper;

    public ContactsGetHandler(CosmosContext db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    public async Task<List<ContactsGetResult>> Handle(ContactsGetQuery request, CancellationToken cancellationToken)
    {
        var queryDef = new QueryDefinition("SELECT * FROM c WHERE c.docType = 'Contact'");
        var contacts = await this.db.Contacts.Query<Contact>(queryDef).ConfigureAwait(false);
        var contactsResult = this.mapper.Map<List<Contact>, List<ContactsGetResult>>(contacts);
        return await Task.FromResult(contactsResult).ConfigureAwait(false);
    }
}
