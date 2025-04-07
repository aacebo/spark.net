using Microsoft.Spark.AI.Annotations;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps;
using Microsoft.Spark.AspNetCore;

namespace Samples.Lights;

[Prompt]
[Prompt.Description("manage light status")]
[Prompt.Instructions(
    "The following is a conversation with an AI assistant.",
    "The assistant can turn the lights on or off.",
    "The lights are currently off."
)]
public class LightsPrompt
{
    private IContext<IActivity> Context => _services.GetSparkContext();
    private readonly IServiceProvider _services;

    public LightsPrompt(IServiceProvider provider)
    {
        _services = provider;
    }

    [Function]
    [Function.Description("get the current light status")]
    public bool GetLightStatus()
    {
        Console.WriteLine(Context.Activity);
        return false;
    }

    [Function]
    [Function.Description("turn the lights on")]
    public void LightsOn()
    {

    }

    [Function]
    [Function.Description("turn the lights off")]
    public void LightsOff()
    {

    }
}