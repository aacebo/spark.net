using System.Text.Json.Serialization;

namespace Microsoft.Spark.Common.Json;

public class TrueTypeJsonAttribute<T>() : JsonConverterAttribute(typeof(TrueTypeJsonConverter<T>)) where T : notnull
{

}