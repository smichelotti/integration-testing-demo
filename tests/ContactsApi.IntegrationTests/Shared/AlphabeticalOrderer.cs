using Xunit.Abstractions;
using Xunit.Sdk;

namespace ContactsApi.IntegrationTests.Shared;

public class AlphabeticalOrderer : ITestCaseOrderer
{
    public const string Name = "ContactsApi.IntegrationTests.Shared.AlphabeticalOrderer";
    public const string Assembly = "ContactsApi.IntegrationTests";

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        var result = testCases.ToList();
        result.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
        return result;
    }

    //public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
    //        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase =>
    //        testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
}
