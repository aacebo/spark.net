using System.Text.Json;

using Microsoft.Spark.AI.Models.OpenAI;
using Microsoft.Spark.Apps;
using Microsoft.Spark.AspNetCore;

using Samples.Lights;

var builder = WebApplication.CreateBuilder(args);
var apiKey = builder.Configuration.GetRequiredSection("OpenAI").GetValue<string>("ApiKey") ?? throw new Exception("OpenAI.ApiKey is required");
var model = new Chat("gpt-4o", apiKey);

builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));
builder.Services.AddScoped<LightsPrompt>();
builder.Services.AddScoped(provider =>
{
    var logger = provider.GetRequiredService<Microsoft.Spark.Common.Logging.ILogger>();
    var lightsPrompt = provider.GetRequiredService<LightsPrompt>();
    var prompt = OpenAIChatPrompt.From(model, lightsPrompt);

    prompt.OnError(ex => logger.Error(ex.ToString()));
    return prompt;
});

var app = builder.Build();
var spark = app.UseSpark();

spark.OnMessage("/history", async context =>
{
    var state = State.From(context);
    await context.Send(JsonSerializer.Serialize(state.Messages, new JsonSerializerOptions()
    {
        WriteIndented = true
    }));
});

spark.OnMessage(async context =>
{
    var state = State.From(context);
    var prompt = app.Services.GetOpenAIChatPrompt();

    await prompt.Send(context.Activity.Text, new() { Messages = state.Messages }, (chunk) => Task.Run(() =>
    {
        context.Stream.Emit(chunk);
    }));

    state.Save(context);
});

app.Run();