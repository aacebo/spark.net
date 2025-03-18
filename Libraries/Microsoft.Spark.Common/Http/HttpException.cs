using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Common.Http;

public class HttpException : Exception
{
    public required HttpResponseHeaders Headers { get; set; }
    public required HttpStatusCode StatusCode { get; set; }
    public Dictionary<string, object>? Body { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(Body, new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}