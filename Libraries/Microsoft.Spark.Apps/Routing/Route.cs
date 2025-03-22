using System.Reflection;

using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

public interface IRoute
{
    public bool Select(Activity activity);
    public Task Invoke(IContext<Activity> context);
}

public class Route : IRoute
{
    public string? Name { get; set; }
    public required Func<Activity, bool> Selector { get; set; }
    public required Func<IContext<Activity>, Task> Handler { get; set; }

    public bool Select(Activity activity) => Selector(activity);
    public Task Invoke(IContext<Activity> context) => Handler(context);
}

public class AttributeRoute : IRoute
{
    public ActivityType? Type { get; set; }
    public required MethodInfo Method { get; set; }

    public bool Select(Activity activity) => Type == null || Type.Equals(activity.Type);
    public async Task Invoke(IContext<Activity> context)
    {
        object? res = null;

        if (Type == null)
        {
            res = Method.Invoke(null, [context]);
        }
        else if (Type.IsTyping)
        {
            res = Method.Invoke(null, [context.ToActivityType<TypingActivity>()]);
        }
        else if (Type.IsInstallUpdate)
        {
            res = Method.Invoke(null, [context.ToActivityType<InstallUpdateActivity>()]);
        }
        else if (Type.IsMessage)
        {
            res = Method.Invoke(null, [context.ToActivityType<MessageActivity>()]);
        }
        else if (Type.IsMessageUpdate)
        {
            res = Method.Invoke(null, [context.ToActivityType<MessageUpdateActivity>()]);
        }
        else if (Type.IsMessageReaction)
        {
            res = Method.Invoke(null, [context.ToActivityType<MessageReactionActivity>()]);
        }
        else if (Type.IsMessageDelete)
        {
            res = Method.Invoke(null, [context.ToActivityType<MessageDeleteActivity>()]);
        }
        else if (Type.IsConversationUpdate)
        {
            res = Method.Invoke(null, [context.ToActivityType<ConversationUpdateActivity>()]);
        }
        else if (Type.IsEndOfConversation)
        {
            res = Method.Invoke(null, [context.ToActivityType<EndOfConversationActivity>()]);
        }
        else if (Type.IsCommand)
        {
            res = Method.Invoke(null, [context.ToActivityType<CommandActivity>()]);
        }
        else if (Type.IsCommandResult)
        {
            res = Method.Invoke(null, [context.ToActivityType<CommandResultActivity>()]);
        }

        if (res is Task task)
            await task;
    }
}