namespace ContactsApi.Controllers;

[Route("api/contacts")]
[ApiController]
public class ContactPutController : BaseApiController
{
    public ContactPutController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("{id}")]
    [ProducesJson]
    [SwaggerOperation(Summary = "Updates the Contact", Tags = new[] { "Contacts" })]
    [SwaggerResponse(200, "Returns the updated Contact", typeof(ContactPutResult))]
    [SwaggerResponse(400, "Invalid Contact.")]
    [SwaggerResponse(404, "Contact Not Found.")]
    public async Task<IActionResult> Get([FromRoute]ContactPutCommand command)
    {
        var result = await this.Mediator.Send(command).ConfigureAwait(false);
        return this.OkOrNotFound(result);
    }
}
