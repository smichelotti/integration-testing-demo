namespace ContactsApi.Controllers;

public class ContactGetMappingProfile : Profile
{
    public ContactGetMappingProfile()
    {
        this.CreateMap<Contact, ContactGetResult>();
    }
}
