namespace Microsoft.Spark.Apps;

public partial class App
{
    protected event EventHandler<Events.ErrorEventArgs> Error = (sender, e) => { };

    protected void OnError(object? sender, Events.ErrorEventArgs e)
    {

    }
}