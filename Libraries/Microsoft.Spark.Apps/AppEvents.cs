namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public IApp OnError(EventHandler<Events.ErrorEventArgs> handler);
}

public partial class App
{
    protected event EventHandler<Events.ErrorEventArgs> Error = (sender, e) => { };

    public IApp OnError(EventHandler<Events.ErrorEventArgs> handler)
    {
        Error += handler;
        return this;
    }
}