using System.Reflection;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps.Routing;

namespace Microsoft.Spark.Apps;

public partial interface IApp : IAppRouting;

public partial class App : AppRouting
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

                    Router.Register(attribute.Name, async (IContext<Activity> context) =>
                    {
                        if (!attribute.Type.IsAssignableFrom(context.Activity.GetType())) return;

                        object? res = null;

                        if (attribute.Name == "activity")
                        {
                            res = method.Invoke(null, [context]);
                        }
                        else if (ActivityType.Typing.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<TypingActivity>()]);
                        }
                        else if (ActivityType.InstallUpdate.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<InstallUpdateActivity>()]);
                        }
                        else if (ActivityType.Message.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageActivity>()]);
                        }
                        else if (ActivityType.MessageUpdate.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageUpdateActivity>()]);
                        }
                        else if (ActivityType.MessageReaction.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageReactionActivity>()]);
                        }
                        else if (ActivityType.MessageDelete.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<MessageDeleteActivity>()]);
                        }
                        else if (ActivityType.ConversationUpdate.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<ConversationUpdateActivity>()]);
                        }
                        else if (ActivityType.EndOfConversation.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<EndOfConversationActivity>()]);
                        }
                        else if (ActivityType.Command.Equals(attribute.Name))
                        {
                            res = method.Invoke(null, [context.ToActivityType<CommandActivity>()]);
                        }
                        else if (ActivityType.CommandResult.Equals(attribute.Name))
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