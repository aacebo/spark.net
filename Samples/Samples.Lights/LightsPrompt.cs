using Microsoft.Spark.AI.Annotations;
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
    protected SparkContext _context;

    public LightsPrompt(SparkContext context)
    {
        _context = context;
    }

    [Function]
    [Function.Description("get the current light status")]
    public bool GetLightStatus()
    {
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