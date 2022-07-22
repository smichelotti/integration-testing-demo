using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.IntegrationTests.Shared;

public class ProblemDetailsExt : ProblemDetails
{
    public Dictionary<string, string[]> Errors { get; set; }
}
