namespace Microsoft.Spark.Agents;

public interface ISerializer
{
    public string Serialize(IMessage message);
    public string? TrySerialize(IMessage message);
    public IMessage Deserialize(string payload);
    public IMessage? TryDeserialize(string payload);
}