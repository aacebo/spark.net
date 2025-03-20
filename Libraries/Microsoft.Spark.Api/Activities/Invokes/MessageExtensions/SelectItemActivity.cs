using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly Name SelectItem = new("composeExtension/selectItem");
        public bool IsSelectItem => SelectItem.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class SelectItemActivity() : InvokeActivity(Name.MessageExtensions.SelectItem)
    {
    }
}