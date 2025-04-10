namespace Microsoft.Spark.Agents;

public delegate void Ack();
public interface ITransport
{
    public Task Send(IMessage message, CancellationToken cancellationToken = default);
    public void Ack(string id);

    public void OnMessage(Action<IMessage, Ack> onMessage);
    public void OnMessage(Func<IMessage, Ack, Task> onMessage);
}