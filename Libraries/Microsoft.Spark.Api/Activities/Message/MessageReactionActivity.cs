using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities.Message;

public class MessageReactionActivity() : Activity("messageReaction")
{
    [JsonPropertyName("reactionsAdded")]
    [JsonPropertyOrder(121)]
    public IList<MessageReaction>? ReactionsAdded { get; set; }

    [JsonPropertyName("reactionsRemoved")]
    [JsonPropertyOrder(122)]
    public IList<MessageReaction>? ReactionsRemoved { get; set; }

    public MessageReactionActivity AddReaction(MessageReaction reaction)
    {
        if (ReactionsAdded == null)
        {
            ReactionsAdded = [];
        }

        ReactionsAdded.Add(reaction);
        return this;
    }

    public MessageReactionActivity RemoveReaction(MessageReaction reaction)
    {
        if (ReactionsRemoved == null)
        {
            ReactionsRemoved = [];
        }

        if (ReactionsAdded != null)
        {
            var i = ReactionsAdded.ToList().FindIndex(r =>
                r.Type.Equals(reaction.Type) && r.User?.Id == reaction.User?.Id
            );

            if (i > -1)
            {
                ReactionsAdded.RemoveAt(i);
                return this;
            }
        }

        ReactionsRemoved.Add(reaction);
        return this;
    }
}