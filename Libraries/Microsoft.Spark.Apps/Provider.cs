namespace Microsoft.Spark.Apps;

internal interface IProvider
{
    public object Resolve();
}

internal class ValueProvider(object value) : IProvider
{
    public object UseValue { get; set; } = value;

    public object Resolve()
    {
        return UseValue;
    }
}

internal class FactoryProvider(FactoryProvider.FactoryProviderDelegate factory) : IProvider
{
    public FactoryProviderDelegate UseFactory { get; set; } = factory;

    public object Resolve()
    {
        return UseFactory();
    }

    public delegate object FactoryProviderDelegate();
}