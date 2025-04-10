namespace Microsoft.Spark.Agents;

public static partial class Messages
{
    public static TextMessage Text(string content) => new(content);
}

public class TextMessage(string text) : IMessage
{
    public IHeaders Headers { get; set; } = new HeaderCollection();
    public string Type => "text";
    public object Content { get; set; } = text;
}