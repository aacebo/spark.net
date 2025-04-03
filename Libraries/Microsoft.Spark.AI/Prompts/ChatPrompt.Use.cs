using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI.Prompts;

public partial class ChatPrompt<TOptions>
{
    /// <summary>
    /// arguments passed to a chat prompt function
    /// by the model
    /// </summary>
    internal class Args
    {
        [JsonPropertyName("text")]
        [JsonPropertyOrder(0)]
        public required string Text { get; set; }
    }

    /// <summary>
    /// provide another chat prompt to be used
    /// as a function
    /// </summary>
    /// <param name="prompt">the chat prompt</param>
    public ChatPrompt<TOptions> Use(IChatPrompt<TOptions> prompt)
    {
        var schema = Schemas.Object().Property(
            "text",
            Schemas.String().WithDescription("the text to send to the assistant"),
            required: true
        );

        Functions[prompt.Name] = new Function<Args>(
            prompt.Name,
            prompt.Description,
            schema,
            async (args) =>
            {
                var res = await prompt.Send(args.Text);
                return res.Content;
            }  
        );

        return this;
    }
}