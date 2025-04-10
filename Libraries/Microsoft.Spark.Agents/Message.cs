using Microsoft.Spark.Common.Json;

namespace Microsoft.Spark.Agents;

/// <summary>
/// the way agents communicate with
/// one another
/// </summary>
[TrueTypeJson<IMessage>]
public interface IMessage
{
    public IHeaders Headers { get; }
    public string? Subject { get; }
    public IList<IContent> Content { get; }
    public IList<ITaskRequest> Tasks { get; }

    public IMessage Add(IContent content);
    public IMessage Add(ITaskRequest request);
}

public class Message : IMessage
{
    public IHeaders Headers { get; set; }
    public string? Subject { get; set; }
    public IList<IContent> Content { get; set; }
    public IList<ITaskRequest> Tasks { get; set; }

    public Message(string? subject = null)
    {
        Headers = new HeaderCollection();
        Subject = subject;
        Content = [];
        Tasks = [];
    }

    public Message(params IContent[] content)
    {
        Headers = new HeaderCollection();
        Content = content;
        Tasks = [];
    }

    public Message(params ITaskRequest[] tasks)
    {
        Headers = new HeaderCollection();
        Content = [];
        Tasks = tasks;
    }

    public IMessage Add(IContent content)
    {
        Content.Add(content);
        return this;
    }

    public IMessage Add(ITaskRequest request)
    {
        Tasks.Add(request);
        return this;
    }

    public Message AddHeader(string key, params string[] value)
    {
        Headers.Add(key, value);
        return this;
    }
}