namespace ContactsApi.Controllers;

[Route("api/contacts")]
[ApiController]
public class ContactsGetController : BaseApiController
{
    public ContactsGetController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet]
    [ProducesJson]
    [SwaggerOperation(Summary = "Retrieves the list of Contacts", Tags = new[] { "Contacts" })]
    [SwaggerResponse(200, "Retrieves the list of Contacts", typeof(List<ContactsGetResult>))]
    public async Task<IActionResult> Get()
    {
        var result = await this.Mediator.Send(new ContactsGetQuery()).ConfigureAwait(false);
        return this.OkOrNotFound(result);
    }
}
