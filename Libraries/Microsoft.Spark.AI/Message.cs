using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.AI;

/// <summary>
/// some message sent to or from the LLM
/// via a Model
/// </summary>
public interface IMessage : IMessage<object?>;

/// <summary>
/// some message sent to or from the LLM
/// via a Model
/// </summary>
public interface IMessage<T>
{
    /// <summary>
    /// the role of the message, ie
    /// who sent the message
    /// </summary>
    public Role Role { get; }

    /// <summary>
    /// the content of the message
    /// </summary>
    public T Content { get; }
}

[JsonConverter(typeof(JsonConverter<Role>))]
public class Role(string value) : StringEnum(value)
{
    public static readonly Role User = new("user");
    public bool IsUser => User.Equals(Value);

    public static readonly Role Model = new("model");
    public bool IsModel => Model.Equals(Value);

    public static readonly Role Developer = new("developer");
    public bool IsDeveloper => Developer.Equals(Value);

    public static readonly Role Function = new("function");
    public bool IsFunction => Function.Equals(Value);
}