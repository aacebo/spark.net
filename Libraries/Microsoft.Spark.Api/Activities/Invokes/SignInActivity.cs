namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any SignIn Activity
/// </summary>
public abstract class SignInActivity(Name.SignIn name) : InvokeActivity(new(name.Value))
{
    public SignIn.TokenExchangeActivity ToTokenExchange() => (SignIn.TokenExchangeActivity)this;
    public SignIn.VerifyStateActivity ToVerifyState() => (SignIn.VerifyStateActivity)this;
}