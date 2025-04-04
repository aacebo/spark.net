using Microsoft.Spark.AI.Prompts;
using Microsoft.Spark.AI.Models.OpenAI;
using Microsoft.Spark.Apps;
using Microsoft.Spark.AspNetCore;
using OpenAI.Chat;

using Samples.Lights;

var builder = WebApplication.CreateBuilder(args);
builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));

var app = builder.Build();
var spark = app.Services.GetService<IApp>()!;
var apiKey = app.Configuration.GetRequiredSection("OpenAI").GetValue<string>("ApiKey");

if (apiKey == null)
{
    throw new Exception("OpenAI.ApiKey is required");
}

var model = new Chat("gpt-4o", apiKey);

spark.OnMessage(async context =>
{
    var state = (State?)context.Storage.Get(context.Activity.From.Id) ?? new State();
    var prompt = new ChatPrompt<ChatCompletionOptions>(
        model,
        new ChatPromptOptions().WithInstructions(
        [
            "The following is a conversation with an AI assistant.",
            "The assistant can turn the lights on or off.",
            "The lights are currently off."
        ])
    ).Function(
        "get_light_status",
        "get the current light status",
        (_) => Task.FromResult<object?>(state.Status)
    ).Function(
        "lights_on",
        "turn the lighnts on",
        (_) =>
        {
            state.Status = true;
            context.Storage.Set(context.Activity.From.Id, state);
            return Task.FromResult<object?>(null);
        }
    ).Function(
        "lights_off",
        "turn the lights off",
        (_) =>
        {
            state.Status = false;
            context.Storage.Set(context.Activity.From.Id, state);
            return Task.FromResult<object?>(null);
        }
    );

    await prompt.Send(context.Activity.Text, new()
    {
        OnChunk = (chunk) => Task.Run(() => context.Stream.Emit(chunk))
    });
});

app.UseSpark();
app.Run();