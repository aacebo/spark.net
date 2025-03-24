using System.Reflection;

using Microsoft.Spark.Apps.Routing;
using Microsoft.Spark.Common.Http;

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

                foreach (object attr in attrs)
                {
                    var attribute = (ActivityAttribute)attr;
                    var route = new AttributeRoute() { Attr = attribute, Method = method };
                    var result = route.Validate();

                    if (!result.Valid) throw new InvalidOperationException(result.ToString());
                    Router.Register(route);
                }
            }
        }
    }

    protected async Task<Response> OnTokenExchangeActivity(IContext<Api.Activities.Invokes.SignIn.TokenExchangeActivity> context)
    {
        var key = $"auth/{context.Ref.Conversation.Id}/{context.Activity.From.Id}";

        try
        {
            await Storage.SetAsync(key, context.Activity.Value.ConnectionName);
            var res = await context.Api.Users.Token.ExchangeAsync(new Api.Clients.UserTokenClient.ExchangeTokenRequest()
            {
                ChannelId = context.Activity.ChannelId,
                ConnectionName = context.Activity.Value.ConnectionName,
                UserId = context.Activity.From.Id,
                ExchangeRequest = new Api.TokenExchange.Request()
                {
                    Token = context.Activity.Value.Token
                }
            });

            return new Response(System.Net.HttpStatusCode.OK);
        }
        catch (HttpException ex)
        {
            if (!ex.StatusCode.Equals(404) && !ex.StatusCode.Equals(400) && !ex.StatusCode.Equals(412))
            {
                await ErrorEvent(this, new() { Error = ex, Logger = context.Log });
                return new Response(ex.StatusCode);
            }

            return new Response(System.Net.HttpStatusCode.PreconditionFailed, new Api.TokenExchange.InvokeResponse()
            {
                Id = context.Activity.Value.Id,
                ConnectionName = context.Activity.Value.ConnectionName,
                FailureDetail = "unable to exchange token",
            });
        }
    }

    protected async Task<Response> OnVerifyStateActivity(IContext<Api.Activities.Invokes.SignIn.VerifyStateActivity> context)
    {
        var key = $"auth/{context.Ref.Conversation.Id}/{context.Activity.From.Id}";

        try
        {
            var connectionName = (string?)await Storage.GetAsync(key);

            if (connectionName == null || context.Activity.Value.State == null)
            {
                context.Log.Warn($"auth state not found for conversation '{context.Ref.Conversation.Id}' and user '{context.Activity.From.Id}'");
                return new Response(System.Net.HttpStatusCode.NotFound);
            }

            var res = await context.Api.Users.Token.GetAsync(new Api.Clients.UserTokenClient.GetTokenRequest()
            {
                ChannelId = context.Activity.ChannelId,
                UserId = context.Activity.From.Id,
                ConnectionName = connectionName,
                Code = context.Activity.Value.State
            });

            await Storage.DeleteAsync(key);
            return new Response(System.Net.HttpStatusCode.OK);
        }
        catch (HttpException ex)
        {
            if (!ex.StatusCode.Equals(404) && !ex.StatusCode.Equals(400) && !ex.StatusCode.Equals(412))
            {
                await ErrorEvent(this, new() { Error = ex, Logger = context.Log });
                return new Response(ex.StatusCode);
            }

            return new Response(System.Net.HttpStatusCode.PreconditionFailed);
        }
    }
}