using System.ClientModel;

using Microsoft.Spark.Common.Logging;

using OpenAI;
using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public partial class Chat : IChatModel<ChatCompletionOptions>
{
    public string Name => throw new NotImplementedException();

    /// <summary>
    /// the OpenAI client used to
    /// make requests
    /// </summary>
    public OpenAIClient Client { get; set; }

    /// <summary>
    /// the model name
    /// </summary>
    protected string Model { get; set; }

    /// <summary>
    /// the logger instance
    /// </summary>
    protected ILogger? Logger { get; set; }

    public Chat(string model, string apiKey, Options? options = null)
    {
        Model = model;
        Client = new(new ApiKeyCredential(apiKey), options ?? new());
        Logger = (options?.Logger ?? new ConsoleLogger<Chat>()).Child(model);
    }

    public Chat(string model, ApiKeyCredential apiKey, Options? options = null)
    {
        Model = model;
        Client = new(apiKey, options ?? new());
        Logger = (options?.Logger ?? new ConsoleLogger<Chat>()).Child(model);
    }
}
