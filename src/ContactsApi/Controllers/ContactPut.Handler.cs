namespace ContactsApi.Controllers;

public class ContactPutHandler : IRequestHandler<ContactPutCommand, ContactPutResult>
{
    private readonly CosmosContext db;
    private readonly IGeoLocationService geoService;
    private readonly IMapper mapper;

    public ContactPutHandler(CosmosContext db, IGeoLocationService geoService, IMapper mapper)
    {
        this.db = db;
        this.geoService = geoService;
        this.mapper = mapper;
    }

    public async Task<ContactPutResult> Handle([FromBody]ContactPutCommand command, CancellationToken cancellationToken)
    {
        var contact = this.mapper.Map<Contact>(command.Contact);
        contact.Id = command.Id;
        var point = this.geoService.GetLatLongFromAddress(new(contact.Address.Street, contact.Address.City, contact.Address.State));
        contact.Address.Latitude = point.Latitude;
        contact.Address.Longitude = point.Longitude; 
        var savedContact = await this.db.Contacts.UpdateItem(command.Id, "Contact", contact);
        var response = this.mapper.Map<ContactPutResult>(savedContact);
        return await Task.FromResult(response).ConfigureAwait(false);
    }
}
