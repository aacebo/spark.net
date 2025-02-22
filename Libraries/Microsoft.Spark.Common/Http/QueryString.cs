using System.Text;
using System.Web;

namespace Microsoft.Spark.Common.Http;

public static class QueryString
{
    public static string Serialize<T>(T value) where T : class
    {
        var properties = value.GetType().GetProperties();
        var parts = new List<string>();

        foreach (var property in properties)
        {
            var builder = new StringBuilder();
            builder.Append(HttpUtility.UrlEncode(property.Name));
            builder.Append('=');
            builder.Append(HttpUtility.UrlEncode(property.GetValue(value)?.ToString()));
            parts.Add(builder.ToString());
        }

        return string.Join('&', parts);
    }
}