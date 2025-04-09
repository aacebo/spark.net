namespace Microsoft.Spark.Common;

public interface IBuilder<T>
{
    public T Build();
}