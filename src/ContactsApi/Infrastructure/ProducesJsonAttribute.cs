namespace ContactsApi.Infrastructure;

public class ProducesJsonAttribute : ProducesAttribute
{
    public ProducesJsonAttribute() : base("application/json") { }
}
