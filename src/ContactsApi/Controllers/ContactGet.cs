namespace ContactsApi.Controllers;

[Route("api/contacts")]
[ApiController]
public class ContactGetController : BaseApiController
{
    public ContactGetController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet("{id}")]
    [ProducesJson]
    [SwaggerOperation(Summary = "Retrieves the latest Contact", Tags = new[] { "Contacts" })]
    [SwaggerResponse(200, "Returns the Contact", typeof(ContactGetResult))]
    [SwaggerResponse(404, "Contact Not Found")]
    public async Task<IActionResult> Get([FromRoute]ContactGetQuery query)
    {
        var result = await this.Mediator.Send(query).ConfigureAwait(false);
        return this.OkOrNotFound(result);
    }
}
