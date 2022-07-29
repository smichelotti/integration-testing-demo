namespace ContactsApi.Controllers;

public class ContactPostHandler : IRequestHandler<ContactPostCommand, ContactPostResult>
{
    private readonly CosmosContext db;
    private readonly IGeoLocationClient geoLocationClient;
    private readonly IMapper mapper;

    public ContactPostHandler(CosmosContext db, IGeoLocationClient geoLocationClient, IMapper mapper)
    {
        this.db = db;
        this.geoLocationClient = geoLocationClient;
        this.mapper = mapper;
    }

    public async Task<ContactPostResult> Handle(ContactPostCommand request, CancellationToken cancellationToken)
    {
        var contact = this.mapper.Map<Contact>(request);
        var point = this.geoLocationClient.GetLatLongFromAddress(new(contact.Address.Street, contact.Address.City, contact.Address.State));
        contact.Address.Latitude = point.Latitude;
        contact.Address.Longitude = point.Longitude;
        var savedContact = await this.db.Contacts.InsertItem(contact);
        var response = this.mapper.Map<ContactPostResult>(savedContact);
        return await Task.FromResult(response).ConfigureAwait(false);
    }
}
