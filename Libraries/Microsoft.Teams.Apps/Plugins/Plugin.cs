using Microsoft.Teams.Api;
using Microsoft.Teams.Api.Activities;
using Microsoft.Teams.Api.Auth;

namespace Microsoft.Teams.Apps.Plugins;

/// <summary>
/// a component for extending the base
/// `App` functionality
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// emitted when the plugin encounters an error
    /// </summary>
    public event ErrorEventHandler ErrorEvent;

    /// <summary>
    /// emitted when the plugin receives an activity
    /// </summary>
    public event ActivityEventHandler ActivityEvent;

    /// <summary>
    /// lifecycle method called by the `App` once during initialization
    /// </summary>
    public Task OnInit(IApp app, CancellationToken cancellationToken = default);

    /// <summary>
    /// lifecycle method called by the `App` once during startup
    /// </summary>
    public Task OnStart(IApp app, CancellationToken cancellationToken = default);

    /// <summary>
    /// called by the `App` when an error occurs
    /// </summary>
    public Task OnError(IApp app, IPlugin? plugin, Exception exception, IContext<IActivity>? context, CancellationToken cancellationToken = default);

    /// <summary>
    /// called by the `App` when an activity is received
    /// </summary>
    public Task OnActivity(IApp app, IContext<IActivity> context);

    /// <summary>
    /// called by the `App` when an activity is sent
    /// </summary>
    public Task OnActivitySent(IApp app, IActivity activity, IContext<IActivity> context);

    /// <summary>
    /// called by the `App` when an activity is sent proactively
    /// </summary>
    public Task OnActivitySent(IApp app, ISenderPlugin sender, IActivity activity, ConversationReference reference, CancellationToken cancellationToken = default);

    /// <summary>
    /// called by the `App` when an activity response is sent
    /// </summary>
    public Task OnActivityResponse(IApp app, Response? response, IContext<IActivity> context);

    /// <summary>
    /// process an activity
    /// </summary>
    public Task<Response> Do(IToken token, IActivity activity, CancellationToken cancellationToken = default);

    public delegate Task ErrorEventHandler(IPlugin sender, Exception exception);
    public delegate Task<Response> ActivityEventHandler(ISenderPlugin sender, IToken token, IActivity activity, CancellationToken cancellationToken = default);
}