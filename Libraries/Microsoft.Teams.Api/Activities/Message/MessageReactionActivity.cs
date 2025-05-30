using System.Text.Json.Serialization;

using Microsoft.Teams.Common;

namespace Microsoft.Teams.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType MessageReaction = new("messageReaction");
    public bool IsMessageReaction => MessageReaction.Equals(Value);
}

public class MessageReactionActivity() : Activity(ActivityType.MessageReaction)
{
    [JsonPropertyName("reactionsAdded")]
    [JsonPropertyOrder(121)]
    public IList<Messages.Reaction>? ReactionsAdded { get; set; }

    [JsonPropertyName("reactionsRemoved")]
    [JsonPropertyOrder(122)]
    public IList<Messages.Reaction>? ReactionsRemoved { get; set; }

    public MessageReactionActivity AddReaction(Messages.Reaction reaction)
    {
        if (ReactionsAdded == null)
        {
            ReactionsAdded = [];
        }

        ReactionsAdded.Add(reaction);
        return this;
    }

    public MessageReactionActivity RemoveReaction(Messages.Reaction reaction)
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