namespace ContactsApi.Controllers;

[Route("api/contacts")]
[ApiController]
public class ContactDeleteController : BaseApiController
{
    public ContactDeleteController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpDelete("{id}")]
    [ProducesJson]
    [SwaggerOperation(Summary = "Deletes the given Contact.", Tags = new[] { "Contacts" })]
    [SwaggerResponse(204, "Contact successfully deleted")]
    [SwaggerResponse(404, "Contact Not Found.")]
    public async Task<IActionResult> Delete([FromRoute]ContactDeleteCommand command)
    {
        var result = await this.Mediator.Send(command).ConfigureAwait(false);
        return this.NoContent();
    }
}
