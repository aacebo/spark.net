using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;

namespace Microsoft.Spark.AspNetCore;

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
    public Task<IResult> OnMessage([FromBody] Activity activity, CancellationToken cancellationToken)
    {
        var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault() ?? throw new UnauthorizedAccessException();
        var token = new JsonWebToken(authHeader.Replace("Bearer ", ""));
        var context = HttpContext.RequestServices.GetRequiredService<SparkHttpContext>();
        context.Token = token;
        return _plugin.Do(token, activity, cancellationToken);
    }
}