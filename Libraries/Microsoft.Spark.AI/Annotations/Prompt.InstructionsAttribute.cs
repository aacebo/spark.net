using Microsoft.Spark.AI.Templates;

namespace Microsoft.Spark.AI.Annotations;

public static partial class Prompt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class InstructionsAttribute : Attribute
    {
        /// <summary>
        /// the prompts instructions
        /// </summary>
        public ITemplate Instructions { get; private set; }

        public InstructionsAttribute(params string[] Instructions)
        {
            this.Instructions = new StringTemplate(string.Join("\n", Instructions));
        }
    }
}