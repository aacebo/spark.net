using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class InvokeName : StringEnum
{
    public static readonly InvokeName AdaptiveCardAction = new("adaptiveCard/action");
    public bool IsAdaptiveCardAction => AdaptiveCardAction.Equals(Value);
}

public class AdaptiveCardActionActivity() : InvokeActivity(InvokeName.AdaptiveCardAction)
{

}