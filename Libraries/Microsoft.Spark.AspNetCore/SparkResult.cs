using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public class SparkResult : IResult
{
    private readonly Response _response;

    public SparkResult(Response response)
    {
        _response = response;
    }

    public async Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)_response.Status;
        var body = JsonSerializer.Serialize(_response.Body);
        await context.Response.WriteAsync(body);
    }
}