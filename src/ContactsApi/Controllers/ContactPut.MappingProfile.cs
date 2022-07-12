namespace ContactsApi.Controllers;

public class ContactPutMappingProfile : Profile
{
    public ContactPutMappingProfile()
    {
        this.CreateMap<ContactPut, Contact>();
        this.CreateMap<AddressPutCommand, Address>();
        this.CreateMap<Contact, ContactPutResult>();
        this.CreateMap<Address, AddressPutResult>();
    }
}
