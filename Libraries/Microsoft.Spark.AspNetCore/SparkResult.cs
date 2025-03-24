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

    public Task ExecuteAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)_response.Status;
        return context.Response.WriteAsJsonAsync(_response.Body);
    }
}