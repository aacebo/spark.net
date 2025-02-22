using System.Text.Json;

namespace Microsoft.Spark.Common.Tests;

public class StringEnumTests
{
    [Fact]
    public void JsonSerialize()
    {
        var value = new StringEnum("test");
        var json = JsonSerializer.Serialize(value);

        Assert.Equal("\"test\"", json);

        var obj = new Dictionary<string, dynamic>()
        {
            { "hello", new StringEnum("world") }
        };

        json = JsonSerializer.Serialize(obj);
        Assert.Equal("{\"hello\":\"world\"}", json);
    }
}