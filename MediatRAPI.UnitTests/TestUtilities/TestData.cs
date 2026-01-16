using System.Collections;

namespace MediatRAPI.UnitTests.TestUtilities;

public class GlobalUsings
{
}

// Test data generators for common test scenarios
public class CustomerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "John Doe", "john.doe@example.com" };
        yield return new object[] { "Jane Smith", "jane.smith@company.co.uk" };
        yield return new object[] { "Bob Wilson", "bob.wilson123@domain.org" };
        yield return new object[] { "Alice Johnson", "alice@subdomain.example.com" };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class InvalidEmailTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "invalid-email" };
        yield return new object[] { "@example.com" };
        yield return new object[] { "test@" };
        yield return new object[] { "test.example.com" };
        yield return new object[] { "test@.com" };
        yield return new object[] { "test@domain." };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}