using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps.Extensions;

namespace Microsoft.Spark.Plugins.AspNetCore;

[Route("api")]
[ApiController]
public class SparkController : ControllerBase
{
    private readonly AspNetCorePlugin _plugin;

    public SparkController(AspNetCorePlugin plugin)
    {
        _plugin = plugin;
    }

    [HttpPost("messages")]
    public async Task<IResult> OnMessage([FromBody] Activity activity, CancellationToken cancellationToken)
    {
        var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault() ?? throw new UnauthorizedAccessException();
        var token = new JsonWebToken(authHeader.Replace("Bearer ", ""));
        var context = HttpContext.RequestServices.GetRequiredService<SparkContext>();
        context.Token = token;
        var res = await _plugin.Do(token, activity, cancellationToken);
        return Results.Json(res.Body, statusCode: (int)res.Status);
    }
}