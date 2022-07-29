namespace ContactsApi.Controllers;

[Route("api/contacts")]
[ApiController]
public class ContactPostController : BaseApiController
{
    public ContactPostController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesJson]
    [SwaggerOperation(Summary = "Inserts the new Contact", Tags = new[] { "Contacts" })]
    [SwaggerResponse(200, "Returns the new Contact", typeof(ContactPostResult))]
    [SwaggerResponse(400, "Invalid Contact.")]
    public async Task<IActionResult> Post(ContactPostCommand command)
    {
        var result = await this.Mediator.Send(command).ConfigureAwait(false);
        return this.Created($"/api/contacts/{result.Id}", result);
    }
}
