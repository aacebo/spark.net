namespace Microsoft.Spark.AI;

/// <summary>
/// defines a block of code that
/// can be called by a model
/// </summary>
public interface IFunction : IFunction<object>;

/// <summary>
/// defines a block of code that
/// can be called by a model
/// </summary>
public interface IFunction<T>
{
    /// <summary>
    /// the unique name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// a description of what the function
    /// should be used for
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// the Json Schema representing what
    /// parameters the function accepts
    /// </summary>
    public ISchema? Parameters { get; }

    /// <summary>
    /// called by the model to invoke your function
    /// </summary>
    /// <param name="args">the arguments</param>
    /// <returns>
    /// your functions response, which will be serialized to Json
    /// if it is not already a string. It then gets returned to the model
    /// for processing.
    /// </returns>
    public Task<object?> Invoke(T? args);
}

/// <summary>
/// defines a block of code that
/// can be called by a model
/// </summary>
public class Function : Function<object>
{
    public Function(string name, string description, Func<object?, Task<object?>> handler) : base(name, description, handler)
    {
    }

    public Function(string name, string description, ISchema parameters, Func<object?, Task<object?>> handler) : base(name, description, parameters, handler)
    {
    }
}

/// <summary>
/// defines a block of code that
/// can be called by a model
/// </summary>
public class Function<T> : IFunction<T>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ISchema? Parameters { get; set; }
    public Func<T?, Task<object?>> Handler { get; set; }

    public Function(string name, string description, Func<T?, Task<object?>> handler)
    {
        Name = name;
        Description = description;
        Handler = handler;
    }

    public Function(string name, string description, ISchema parameters, Func<T?, Task<object?>> handler)
    {
        Name = name;
        Description = description;
        Parameters = parameters;
        Handler = handler;
    }

    public Task<object?> Invoke(T? args)
    {
        return Handler(args);
    }
}