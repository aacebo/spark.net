using System.Text.Json;

using Microsoft.Spark.AI.Models.OpenAI;
using Microsoft.Spark.AspNetCore;

using Samples.Lights;

var builder = WebApplication.CreateBuilder(args);
builder.AddSpark().AddOpenAI<LightsPrompt>();

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