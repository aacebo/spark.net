using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

[JsonConverter(typeof(JsonConverter<Name>))]
public partial class Name(string value) : StringEnum(value)
{
}