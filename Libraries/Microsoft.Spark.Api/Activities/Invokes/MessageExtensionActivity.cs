namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Message Extension Activity
/// </summary>
public abstract class MessageExtensionActivity(Name.MessageExtensions name) : InvokeActivity(new(name.Value))
{
    public MessageExtensions.AnonQueryLinkActivity ToAnonQueryLink() => (MessageExtensions.AnonQueryLinkActivity)this;
    public MessageExtensions.CardButtonClickedActivity ToCardButtonClicked() => (MessageExtensions.CardButtonClickedActivity)this;
    public MessageExtensions.FetchTaskActivity ToFetchTask() => (MessageExtensions.FetchTaskActivity)this;
    public MessageExtensions.QueryActivity ToQuery() => (MessageExtensions.QueryActivity)this;
    public MessageExtensions.QueryLinkActivity ToQueryLink() => (MessageExtensions.QueryLinkActivity)this;
    public MessageExtensions.QuerySettingsUrlActivity ToQuerySettingsUrl() => (MessageExtensions.QuerySettingsUrlActivity)this;
    public MessageExtensions.SelectItemActivity ToSelectItem() => (MessageExtensions.SelectItemActivity)this;
    public MessageExtensions.SettingActivity ToSetting() => (MessageExtensions.SettingActivity)this;
    public MessageExtensions.SubmitActionActivity ToSubmitAction() => (MessageExtensions.SubmitActionActivity)this;
}