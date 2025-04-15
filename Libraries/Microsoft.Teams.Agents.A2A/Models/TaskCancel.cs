using System.Text.Json.Serialization;

using Microsoft.Teams.Agents.A2A.Json.Rpc;

namespace Microsoft.Teams.Agents.A2A.Models;

public static partial class Requests
{
    public static Request CancelTask(TaskCancel data) => new()
    {
        Method = "tasks/cancel",
        Params = data
    };
}

/// <summary>
/// Allows a client to send content to a remote agent to start a new Task, resume an interrupted Task or reopen a completed Task.
/// A Task interrupt may be caused due to an agent requiring additional user input or a runtime error.
/// https://google.github.io/A2A/#/documentation?id=send-a-task
/// </summary>
public class TaskCancel
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("metadata")]
    [JsonPropertyOrder(1)]
    public IDictionary<string, object?> MetaData { get; set; } = new Dictionary<string, object?>();
}