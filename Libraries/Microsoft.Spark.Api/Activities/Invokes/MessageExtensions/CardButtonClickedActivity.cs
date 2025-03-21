using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly MessageExtensions CardButtonClicked = new("composeExtension/onCardButtonClicked");
        public bool IsCardButtonClicked => CardButtonClicked.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class CardButtonClickedActivity() : MessageExtensionActivity(Name.MessageExtensions.CardButtonClicked)
    {

    }
}