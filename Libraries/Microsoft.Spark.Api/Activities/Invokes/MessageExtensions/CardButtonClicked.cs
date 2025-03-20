using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly Name CardButtonClicked = new("composeExtension/onCardButtonClicked");
        public bool IsCardButtonClicked => CardButtonClicked.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class CardButtonClickedActivity() : InvokeActivity(Name.MessageExtensions.CardButtonClicked)
    {

    }
}