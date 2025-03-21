using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class BotSignInClient : Client
{
    public BotSignInClient() : base()
    {

    }

    public BotSignInClient(IHttpClient client) : base(client)
    {

    }

    public BotSignInClient(IHttpRequestOptions options) : base(options)
    {

    }

    public BotSignInClient(IHttpClientFactory factory) : base(factory)
    {

    }

    public async Task<string> GetUrlAsync(GetUrlRequest request)
    {
        var query = QueryString.Serialize(request);
        var req = HttpRequest.Get(
            $"https://token.botframework.com/api/botsignin/GetSignInUrl?{query}"
        );

        var res = await _http.SendAsync(req);
        return res.Body;
    }

    public async Task<SignIn.UrlResponse> GetResourceAsync(GetResourceRequest request)
    {
        var query = QueryString.Serialize(request);
        var req = HttpRequest.Get(
            $"https://token.botframework.com/api/botsignin/GetSignInResource?{query}"
        );

        var res = await _http.SendAsync<SignIn.UrlResponse>(req);
        return res.Body;
    }

    public class GetUrlRequest
    {
        public required string State { get; set; }
        public string? CodeChallenge { get; set; }
        public string? EmulatorUrl { get; set; }
        public string? FinalRedirect { get; set; }
    }

    public class GetResourceRequest
    {
        public required string State { get; set; }
        public string? CodeChallenge { get; set; }
        public string? EmulatorUrl { get; set; }
        public string? FinalRedirect { get; set; }
    }
}