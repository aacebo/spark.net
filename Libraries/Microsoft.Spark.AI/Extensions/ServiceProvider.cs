using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.AI.Models;
using Microsoft.Spark.AI.Prompts;

namespace Microsoft.Spark.AI.Extensions;

public static class ServiceProviderExtensions
{
    public static IModel<TOptions> GetModel<TOptions>(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IModel<TOptions>>();
    }

    public static TModel GetModel<TModel, TOptions>(this IServiceProvider provider) where TModel : IModel<TOptions>
    {
        return provider.GetRequiredService<TModel>();
    }

    public static IChatModel<TOptions> GetChatModel<TOptions>(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IChatModel<TOptions>>();
    }

    public static TChatModel GetChatModel<TChatModel, TOptions>(this IServiceProvider provider) where TChatModel : IChatModel<TOptions>
    {
        return provider.GetRequiredService<TChatModel>();
    }

    public static IPrompt<TOptions> GetPrompt<TOptions>(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IPrompt<TOptions>>();
    }

    public static TPrompt GetPrompt<TPrompt, TOptions>(this IServiceProvider provider) where TPrompt : IPrompt<TOptions>
    {
        return provider.GetRequiredService<TPrompt>();
    }

    public static IChatPrompt<TOptions> GetChatPrompt<TOptions>(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IChatPrompt<TOptions>>();
    }

    public static TChatPrompt GetChatPrompt<TChatPrompt, TOptions>(this IServiceProvider provider) where TChatPrompt : IPrompt<TOptions>
    {
        return provider.GetRequiredService<TChatPrompt>();
    }
}