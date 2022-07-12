namespace ContactsApi.Controllers;

public class ContactPostMappingProfile : Profile
{
    public ContactPostMappingProfile()
    {
        this.CreateMap<ContactPostCommand, Contact>();
        this.CreateMap<AddressPostCommand, Address>();
        this.CreateMap<Contact, ContactPostResult>();
        this.CreateMap<Address, AddressPostResult>();
    }
}
