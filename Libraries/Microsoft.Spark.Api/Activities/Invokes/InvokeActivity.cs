using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

[JsonConverter(typeof(JsonConverter<Name>))]
public partial class Name(string value) : StringEnum(value)
{
    [JsonConverter(typeof(JsonConverter<Configs>))]
    public partial class Configs(string value) : StringEnum(value)
    {

    }

    [JsonConverter(typeof(JsonConverter<MessageExtensions>))]
    public partial class MessageExtensions(string value) : StringEnum(value)
    {

    }

    [JsonConverter(typeof(JsonConverter<Messages>))]
    public partial class Messages(string value) : StringEnum(value)
    {

    }

    [JsonConverter(typeof(JsonConverter<SignIn>))]
    public partial class SignIn(string value) : StringEnum(value)
    {

    }

    [JsonConverter(typeof(JsonConverter<Tabs>))]
    public partial class Tabs(string value) : StringEnum(value)
    {

    }

    [JsonConverter(typeof(JsonConverter<Tasks>))]
    public partial class Tasks(string value) : StringEnum(value)
    {

    }
}