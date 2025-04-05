namespace Microsoft.Spark.AI.Annotations;

public static partial class Function
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// the functions description
        /// </summary>
        public string Description { get; private set; }

        public DescriptionAttribute(params string[] Description)
        {
            this.Description = string.Join("\n", Description);
        }
    }
}