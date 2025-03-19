using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Cards.Actions;

public abstract class Action
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}