namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Message Extension Activity
/// </summary>
public abstract class MessageExtensionInvokeActivity(Name.MessageExtensions name) : InvokeActivity(new(name.Value))
{
    public MessageExtensions.AnonQueryLinkActivity ToAnonQueryLink()
    {
        return (MessageExtensions.AnonQueryLinkActivity)this;
    }

    public MessageExtensions.CardButtonClickedActivity ToCardButtonClicked()
    {
        return (MessageExtensions.CardButtonClickedActivity)this;
    }

    public MessageExtensions.FetchTaskActivity ToFetchTask()
    {
        return (MessageExtensions.FetchTaskActivity)this;
    }

    public MessageExtensions.QueryActivity ToQuery()
    {
        return (MessageExtensions.QueryActivity)this;
    }

    public MessageExtensions.QueryLinkActivity ToQueryLink()
    {
        return (MessageExtensions.QueryLinkActivity)this;
    }

    public MessageExtensions.QuerySettingsUrlActivity ToQuerySettingsUrl()
    {
        return (MessageExtensions.QuerySettingsUrlActivity)this;
    }

    public MessageExtensions.SelectItemActivity ToSelectItem()
    {
        return (MessageExtensions.SelectItemActivity)this;
    }

    public MessageExtensions.SettingActivity ToSetting()
    {
        return (MessageExtensions.SettingActivity)this;
    }

    public MessageExtensions.SubmitActionActivity ToSubmitAction()
    {
        return (MessageExtensions.SubmitActionActivity)this;
    }
}