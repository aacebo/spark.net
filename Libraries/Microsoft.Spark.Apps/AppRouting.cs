using System.Reflection;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps.Routing;

namespace Microsoft.Spark.Apps;

public partial interface IApp : IRoutingModule;

public partial class App : RoutingModule
{
    protected void RegisterAttributeRoutes()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

        foreach (Type type in assembly.GetTypes())
        {
            var methods = type.GetMethods();

            foreach (MethodInfo method in methods)
            {
                var attrs = method.GetCustomAttributes(typeof(ActivityAttribute), true);

                if (attrs.Length == 0) continue;

                var param = method.GetParameters().FirstOrDefault();

                if (param == null)
                {
                    throw new ArgumentException("Activity handlers must have 1 parameter of type `ActivityContext`");
                }

                var generic = param.ParameterType.GenericTypeArguments.FirstOrDefault();

                if (generic == null)
                {
                    throw new ArgumentException("Activity handlers must have 1 parameter of type `ActivityContext`");
                }

                foreach (object attr in attrs)
                {
                    var attribute = (ActivityAttribute)attr;

                    if (!attribute.Type.IsAssignableTo(generic))
                    {
                        throw new ArgumentException($"'{generic.Name}' is not assignable to '{attribute.Type.Name}'");
                    }

                    Router.Register(attribute.Name?.Value ?? "activity", async (IContext<Activity> context) =>
                    {
                        if (!attribute.Type.IsAssignableFrom(context.Activity.GetType())) return;

                        object? res = null;

                        if (attribute.Name == null)
                        {
                            res = method.Invoke(null, [context]);
                        }
                        else if (attribute.Name.IsTyping)
                        {
                            res = method.Invoke(null, [context.ToActivityType<TypingActivity>()]);
                        }
                        else if (attribute.Name.IsInstallUpdate)
                        {
                            res = method.Invoke(null, [context.ToActivityType<InstallUpdateActivity>()]);
                        }
                        else if (attribute.Name.IsMessage)
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageActivity>()]);
                        }
                        else if (attribute.Name.IsMessageUpdate)
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageUpdateActivity>()]);
                        }
                        else if (attribute.Name.IsMessageReaction)
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageReactionActivity>()]);
                        }
                        else if (attribute.Name.IsMessageDelete)
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageDeleteActivity>()]);
                        }
                        else if (attribute.Name.IsConversationUpdate)
                        {
                            res = method.Invoke(null, [context.ToActivityType<ConversationUpdateActivity>()]);
                        }
                        else if (attribute.Name.IsEndOfConversation)
                        {
                            res = method.Invoke(null, [context.ToActivityType<EndOfConversationActivity>()]);
                        }
                        else if (attribute.Name.IsCommand)
                        {
                            res = method.Invoke(null, [context.ToActivityType<CommandActivity>()]);
                        }
                        else if (attribute.Name.IsCommandResult)
                        {
                            res = method.Invoke(null, [context.ToActivityType<CommandResultActivity>()]);
                        }

                        if (res is Task task)
                            await task;
                    });
                }
            }
        }
    }
}