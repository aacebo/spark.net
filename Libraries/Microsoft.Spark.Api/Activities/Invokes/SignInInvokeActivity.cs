namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any SignIn Activity
/// </summary>
public abstract class SignInInvokeActivity(Name.SignIn name) : InvokeActivity(new(name.Value))
{
    public SignIn.TokenExchangeActivity ToTokenExchange()
    {
        return (SignIn.TokenExchangeActivity)this;
    }

    public SignIn.VerifyStateActivity ToVerifyState()
    {
        return (SignIn.VerifyStateActivity)this;
    }
}