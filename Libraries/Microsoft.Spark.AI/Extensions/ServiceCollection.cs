using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.AI.Models;
using Microsoft.Spark.AI.Prompts;

namespace Microsoft.Spark.AI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSparkModel<TOptions>(this IServiceCollection collection, IModel<TOptions> model)
    {
        collection.AddSingleton(model);
        return collection;
    }

    public static IServiceCollection AddSparkChatModel<TOptions>(this IServiceCollection collection, IChatModel<TOptions> model)
    {
        collection.AddSingleton(model);
        return collection;
    }

    public static IServiceCollection AddSparkPrompt<TOptions>(this IServiceCollection collection, IPrompt<TOptions> prompt)
    {
        collection.AddSingleton(prompt);
        return collection;
    }

    public static IServiceCollection AddSparkChatPrompt<TOptions>(this IServiceCollection collection, IChatPrompt<TOptions> prompt)
    {
        collection.AddSingleton(prompt);
        return collection;
    }
}