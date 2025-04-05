using Json.Schema;

namespace Microsoft.Spark.AI.Prompts;

public partial class ChatPrompt<TOptions>
{
    public ChatPrompt<TOptions> Function(IFunction function)
    {
        Functions.Add(function);
        return this;
    }

    public ChatPrompt<TOptions> Function(string name, string? description, Func<object?, Task<object?>> handler)
    {
        Functions.Add(new Function(name, description, handler));
        return this;
    }

    public ChatPrompt<TOptions> Function(string name, string? description, Func<object?, Task> handler)
    {
        Functions.Add(new Function(name, description, async (args) =>
        {
            await handler(args);
            return null;
        }));

        return this;
    }

    public ChatPrompt<TOptions> Function<T>(string name, string? description, Func<T, Task<object?>> handler)
    {
        Functions.Add(new Function<T>(name, description, handler));
        return this;
    }

    public ChatPrompt<TOptions> Function<T>(string name, string? description, Func<T, Task> handler)
    {
        Functions.Add(new Function<T>(name, description, async (args) =>
        {
            await handler(args);
            return null;
        }));

        return this;
    }

    public ChatPrompt<TOptions> Function(string name, string? description, JsonSchema parameters, Func<object?, Task<object?>> handler)
    {
        Functions.Add(new Function(name, description, parameters, handler));
        return this;
    }

    public ChatPrompt<TOptions> Function(string name, string? description, JsonSchema parameters, Func<object?, Task> handler)
    {
        Functions.Add(new Function(name, description, parameters, async (args) =>
        {
            await handler(args);
            return null;
        }));

        return this;
    }

    public ChatPrompt<TOptions> Function<T>(string name, string? description, JsonSchema parameters, Func<T, Task<object?>> handler)
    {
        Functions.Add(new Function<T>(name, description, parameters, handler));
        return this;
    }

    public ChatPrompt<TOptions> Function<T>(string name, string? description, JsonSchema parameters, Func<T, Task> handler)
    {
        Functions.Add(new Function<T>(name, description, parameters, async (args) =>
        {
            await handler(args);
            return null;
        }));

        return this;
    }

    public Task<object?> Invoke(string name, object? args = null)
    {
        var function = Functions.Get(name);

        if (function == null)
        {
            return Task.FromResult<object?>(null);
        }

        return function is Function func ? func.Invoke(args) : throw new InvalidDataException();
    }

    public Task<object?> Invoke<T>(string name, T args)
    {
        var function = Functions.Get(name);

        if (function == null)
        {
            return Task.FromResult<object?>(null);
        }

        return function is Function<T> func ? func.Invoke(args) : throw new InvalidDataException();
    }
}