using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class EventName : StringEnum
{
    public static readonly EventName ReadReceipt = new("application/vnd.microsoft.readReceipt");
    public bool IsReadReceipt => ReadReceipt.Equals(Value);
}

public class ReadReceiptActivity() : EventActivity(EventName.ReadReceipt)
{

}