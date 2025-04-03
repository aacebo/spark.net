using Microsoft.Spark.AI.Models;
using Microsoft.Spark.AI.Templates;

namespace Microsoft.Spark.AI.Prompts;

public partial class ChatPrompt<TOptions>
{
    /// <summary>
    /// ChatPrompt Options
    /// </summary>
    public class Options
    {
        /// <summary>
        /// the name of the prompt
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// the description of the prompt
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// the model to send messages to
        /// </summary>
        public IChatModel<TOptions> Model { get; set; }

        /// <summary>
        /// the defining characteristics/objective
        /// of the prompt. This is commonly used to provide a system prompt.
        /// If you supply the system prompt as part of the messages,
        /// you do not need to supply this option.
        /// </summary>
        public ITemplate? Instructions { get; set; }

        /// <summary>
        /// the conversation history
        /// </summary>
        public IList<IMessage>? Messages { get; set; }

        public Options(IChatModel<TOptions> model)
        {
            Model = model;
        }

        public Options WithName(string value)
        {
            Name = value;
            return this;
        }

        public Options WithDescription(string value)
        {
            Description = value;
            return this;
        }

        public Options WithInstructions(string value)
        {
            Instructions = new StringTemplate(value);
            return this;
        }

        public Options WithInstructions(params string[] value)
        {
            Instructions = new StringTemplate(string.Join("\n", value));
            return this;
        }

        public Options WithInstructions(ITemplate value)
        {
            Instructions = value;
            return this;
        }
    }
}