using System.Net;

namespace Microsoft.Spark.Apps;

/// <summary>
/// Represents a response returned by a bot when it receives an activity.
/// </summary>
public class Response
{
    /// <summary>
    /// The HTTP status code of the response.
    /// </summary>
    public required HttpStatusCode Status { get; set; }

    /// <summary>
    /// Optional. The body of the response.
    /// </summary>
    public object? Body { get; set; }

    public Response(HttpStatusCode status)
    {
        Status = status;
    }

    public Response(HttpStatusCode status, object body)
    {
        Status = status;
        Body = body;
    }
}