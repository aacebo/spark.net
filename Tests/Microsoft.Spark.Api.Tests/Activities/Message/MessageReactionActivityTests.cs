using System.Text.Json;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Activities.Message;

namespace Microsoft.Spark.Api.Tests.Activities.Message;

public class MessageReactionActivityTests
{
    [Fact]
    public void JsonSerialize()
    {
        var activity = new MessageReactionActivity()
        {
            Id = "1",
            From = new()
            {
                Id = "1",
                Name = "test",
                Role = Role.User
            },
            Conversation = new()
            {
                Id = "1",
                Type = ConversationType.Personal
            },
            Recipient = new()
            {
                Id = "2",
                Name = "test-bot",
                Role = Role.Bot
            }
        }.AddReaction(new MessageReaction()
        {
            Type = MessageReactionType.Like,
            User = new MessageUser()
            {
                Id = "100",
                Type = UserIdentityType.AnonymousGuest
            }
        });

        var json = JsonSerializer.Serialize(activity, new JsonSerializerOptions()
        {
            WriteIndented = true,
            IndentSize = 4,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        Assert.Equal(File.ReadAllText(
            @"../../../Json/Activity/Message/MessageReactionActivity.json"
        ), json);
    }

    [Fact]
    public void JsonSerialize_Derived()
    {
        MessageReactionActivity activity = new MessageReactionActivity()
        {
            Id = "1",
            From = new()
            {
                Id = "1",
                Name = "test",
                Role = Role.User
            },
            Conversation = new()
            {
                Id = "1",
                Type = ConversationType.Personal
            },
            Recipient = new()
            {
                Id = "2",
                Name = "test-bot",
                Role = Role.Bot
            }
        }.AddReaction(new MessageReaction()
        {
            Type = MessageReactionType.Like,
            User = new MessageUser()
            {
                Id = "100",
                Type = UserIdentityType.AnonymousGuest
            }
        });

        var json = JsonSerializer.Serialize(activity, new JsonSerializerOptions()
        {
            WriteIndented = true,
            IndentSize = 4,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        Assert.Equal(File.ReadAllText(
            @"../../../Json/Activity/Message/MessageReactionActivity.json"
        ), json);
    }

    [Fact]
    public void JsonSerialize_Derived_Interface()
    {
        Activity activity = new MessageReactionActivity()
        {
            Id = "1",
            From = new()
            {
                Id = "1",
                Name = "test",
                Role = Role.User
            },
            Conversation = new()
            {
                Id = "1",
                Type = ConversationType.Personal
            },
            Recipient = new()
            {
                Id = "2",
                Name = "test-bot",
                Role = Role.Bot
            }
        }.AddReaction(new MessageReaction()
        {
            Type = MessageReactionType.Like,
            User = new MessageUser()
            {
                Id = "100",
                Type = UserIdentityType.AnonymousGuest
            }
        });

        var json = JsonSerializer.Serialize(activity, new JsonSerializerOptions()
        {
            WriteIndented = true,
            IndentSize = 4,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        Assert.Equal(File.ReadAllText(
            @"../../../Json/Activity/Message/MessageReactionActivity.json"
        ), json);
    }

    [Fact]
    public void JsonDeserialize()
    {
        var json = File.ReadAllText(@"../../../Json/Activity/Message/MessageReactionActivity.json");
        var activity = JsonSerializer.Deserialize<MessageReactionActivity>(json);
        var expected = new MessageReactionActivity()
        {
            Id = "1",
            From = new()
            {
                Id = "1",
                Name = "test",
                Role = Role.User
            },
            Conversation = new()
            {
                Id = "1",
                Type = ConversationType.Personal
            },
            Recipient = new()
            {
                Id = "2",
                Name = "test-bot",
                Role = Role.Bot
            }
        }.AddReaction(new MessageReaction()
        {
            Type = MessageReactionType.Like,
            User = new MessageUser()
            {
                Id = "100",
                Type = UserIdentityType.AnonymousGuest
            }
        });

        Assert.Equivalent(expected, activity);
    }

    [Fact]
    public void JsonDeserialize_Derived()
    {
        var json = File.ReadAllText(@"../../../Json/Activity/Message/MessageReactionActivity.json");
        var activity = JsonSerializer.Deserialize<Activity>(json);
        var expected = new MessageReactionActivity()
        {
            Id = "1",
            From = new()
            {
                Id = "1",
                Name = "test",
                Role = Role.User
            },
            Conversation = new()
            {
                Id = "1",
                Type = ConversationType.Personal
            },
            Recipient = new()
            {
                Id = "2",
                Name = "test-bot",
                Role = Role.Bot
            }
        }.AddReaction(new MessageReaction()
        {
            Type = MessageReactionType.Like,
            User = new MessageUser()
            {
                Id = "100",
                Type = UserIdentityType.AnonymousGuest
            }
        });

        Assert.Equivalent(expected, activity);
    }
}