using Microsoft.Spark.Common.Logging;

using OpenAI;

namespace Microsoft.Spark.AI.Models.OpenAI;

public partial class OpenAIChatModel
{
    /// <summary>
    /// the model options
    /// </summary>
    public class Options : OpenAIClientOptions
    {
        /// <summary>
        /// the logger instance
        /// </summary>
        public ILogger? Logger { get; set; }
    }
}