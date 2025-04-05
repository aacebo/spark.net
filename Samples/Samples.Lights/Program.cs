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
    var lightsPrompt = provider.GetRequiredService<LightsPrompt>();
    return ChatPrompt<ChatCompletionOptions>.From(model, lightsPrompt);
});

builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));

var app = builder.Build();
var spark = app.UseSpark();

spark.OnMessage(async context =>
{
    var prompt = app.Services.GetRequiredService<ChatPrompt<ChatCompletionOptions>>();

    await prompt.Send(context.Activity.Text, new()
    {
        OnChunk = (chunk) => Task.Run(() => context.Stream.Emit(chunk))
    });
});

app.Run();