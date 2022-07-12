namespace ContactsApi.Controllers;

public class BaseApiController : ControllerBase
{
    protected IMediator Mediator;

    public BaseApiController(IMediator mediator)
    {
        if (mediator == null)
            throw new ArgumentNullException(nameof(mediator));

        this.Mediator = mediator;
    }

    protected IActionResult OkOrNotFound(object item)
    {
        if (item == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(item);
        }
    }
}
