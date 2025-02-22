using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

[JsonConverter(typeof(JsonConverter<ChannelId>))]
public class ChannelId(string value) : StringEnum(value)
{
    public static readonly ChannelId MsTeams = new("msteams");
    public bool IsMsTeams => MsTeams.Equals(Value);

    public static readonly ChannelId WebChat = new("webchat");
    public bool IsWebChat => WebChat.Equals(Value);
}