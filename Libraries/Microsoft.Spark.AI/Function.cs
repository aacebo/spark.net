namespace Microsoft.Spark.AI;

/// <summary>
/// defines a block of code that
/// can be called by a model
/// </summary>
public interface IFunction
{
    /// <summary>
    /// the unique name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// a description of what the function
    /// should be used for
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// the Json Schema representing what
    /// parameters the function accepts
    /// </summary>
    public ISchema? Parameters { get; }
}

/// <summary>
/// defines a block of code that
/// can be called by a model
/// </summary>
public class Function : Function<object>
{
    public Function(string name, string? description, Func<object?, Task<object?>> handler) : base(name, description, handler)
    {
    }

    public Function(string name, string? description, ISchema parameters, Func<object?, Task<object?>> handler) : base(name, description, parameters, handler)
    {
    }
}

/// <summary>
/// defines a block of code that
/// can be called by a model
/// </summary>
public class Function<T> : IFunction
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public ISchema? Parameters { get; set; }

    internal Func<T, Task<object?>> Handler { get; set; }

    public Function(string name, string? description, Func<T, Task<object?>> handler)
    {
        Name = name;
        Description = description;
        Handler = handler;
    }

    public Function(string name, string? description, ISchema parameters, Func<T, Task<object?>> handler)
    {
        Name = name;
        Description = description;
        Parameters = parameters;
        Handler = handler;
    }

    internal Task<object?> Invoke(T args) => Handler(args);
    internal Task<object?> Invoke(object? args) => Handler((T?)args ?? throw new InvalidDataException());
}