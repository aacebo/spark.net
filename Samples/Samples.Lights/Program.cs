using Microsoft.Spark.AI.Models.OpenAI;
using Microsoft.Spark.AI.Prompts;
using Microsoft.Spark.Apps;
using Microsoft.Spark.AspNetCore;

using OpenAI.Chat;

using Samples.Lights;

var builder = WebApplication.CreateBuilder(args);
var apiKey = builder.Configuration.GetRequiredSection("OpenAI").GetValue<string>("ApiKey") ?? throw new Exception("OpenAI.ApiKey is required");
var model = new Chat("gpt-4o", apiKey);

builder.Services.AddSingleton<LightsPrompt>();
builder.Services.AddSingleton(provider =>
{
    var lightsPrompt = provider.GetService<LightsPrompt>() ?? throw new Exception("LightsPrompt not found");
    return ChatPrompt<ChatCompletionOptions>.From(model, lightsPrompt);
});

builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));

var app = builder.Build();
var spark = app.Services.GetService<IApp>()!;

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
        (_) => Task.FromResult(state.Status)
    ).Function(
        "lights_on",
        "turn the lights on",
        async (_) =>
        {
            state.Status = true;
            await context.Storage.SetAsync(context.Activity.From.Id, state);
        }
    ).Function(
        "lights_off",
        "turn the lights off",
        async (_) =>
        {
            state.Status = false;
            await context.Storage.SetAsync(context.Activity.From.Id, state);
        }
    );

    await prompt.Send(context.Activity.Text, new()
    {
        OnChunk = (chunk) => Task.Run(() => context.Stream.Emit(chunk))
    });
});

app.UseSpark();
app.Run();