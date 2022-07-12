using Xunit.Abstractions;
using Xunit.Sdk;

namespace ContactsApi.IntegrationTests.Shared;

public class AlphabeticalOrderer : ITestCaseOrderer
{
    public const string Name = "ContactsApi.IntegrationTests.Shared.AlphabeticalOrderer";
    public const string Assembly = "ContactsApi.IntegrationTests";

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase 
        => testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
}
