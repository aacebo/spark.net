using Microsoft.AspNetCore.Http;

namespace Microsoft.Spark.AspNetCore;

public class SparkContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, SparkContext spark)
    {
        spark.Http = context;
        
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}