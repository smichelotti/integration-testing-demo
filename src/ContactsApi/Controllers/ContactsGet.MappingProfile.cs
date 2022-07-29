namespace ContactsApi.Controllers;

public class ContactsGetMappingProfile : Profile
{
    public ContactsGetMappingProfile()
    {
        this.CreateMap<Contact, ContactsGetResult>();
        this.CreateMap<Address, AddressGetResult>();
    }
}
