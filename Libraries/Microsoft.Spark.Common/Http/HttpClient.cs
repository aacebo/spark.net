using System.Net.Http.Json;

namespace Microsoft.Spark.Common.Http;

public interface IHttpClient : IDisposable
{
    public IHttpClientOptions Options { get; }

    public Task<IHttpResponse<string>> SendAsync(IHttpRequest request);
    public Task<IHttpResponse<TResponseBody>> SendAsync<TResponseBody>(IHttpRequest request);
    public Task<IHttpResponse<string>> SendAsync(IHttpRequest request, CancellationToken cancellationToken);
    public Task<IHttpResponse<TResponseBody>> SendAsync<TResponseBody>(IHttpRequest request, CancellationToken cancellationToken);
}

public class HttpClient : IHttpClient
{
    public IHttpClientOptions Options { get; }

    protected System.Net.Http.HttpClient _client;

    public HttpClient()
    {
        _client = new System.Net.Http.HttpClient();
        Options = new HttpClientOptions();
        Options.Apply(_client);
    }

    public HttpClient(IHttpClientOptions options)
    {
        _client = new System.Net.Http.HttpClient();
        Options = options;
        Options.Apply(_client);
    }

    public HttpClient(System.Net.Http.HttpClient client)
    {
        _client = client;
        Options = new HttpClientOptions();
        Options.Apply(_client);
    }

    public async Task<IHttpResponse<string>> SendAsync(IHttpRequest request)
    {
        var httpRequest = CreateRequest(request);
        var httpResponse = await _client.SendAsync(httpRequest);
        return await CreateResponse(httpResponse);
    }

    public async Task<IHttpResponse<TResponseBody>> SendAsync<TResponseBody>(IHttpRequest request)
    {
        var httpRequest = CreateRequest(request);
        var httpResponse = await _client.SendAsync(httpRequest);
        return await CreateResponse<TResponseBody>(httpResponse);
    }

    public async Task<IHttpResponse<string>> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
    {
        var httpRequest = CreateRequest(request);
        var httpResponse = await _client.SendAsync(httpRequest, cancellationToken);
        return await CreateResponse(httpResponse, cancellationToken);
    }

    public async Task<IHttpResponse<TResponseBody>> SendAsync<TResponseBody>(IHttpRequest request, CancellationToken cancellationToken)
    {
        var httpRequest = CreateRequest(request);
        var httpResponse = await _client.SendAsync(httpRequest, cancellationToken);
        return await CreateResponse<TResponseBody>(httpResponse, cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    protected HttpRequestMessage CreateRequest(IHttpRequest request)
    {
        var httpRequest = new HttpRequestMessage(
            request.Method,
            request.Url
        );

        Options.Apply(httpRequest);

        foreach (var (key, value) in request.Headers)
        {
            if (key.StartsWith("Content-"))
            {
                httpRequest.Content?.Headers.TryAddWithoutValidation(key, value);
                continue;
            }

            httpRequest.Headers.TryAddWithoutValidation(key, value);
        }

        if (request.Body != null)
        {
            if (request.Body is string stringBody)
            {
                httpRequest.Content = new StringContent(stringBody);
                return httpRequest;
            }

            if (request.Body is IEnumerable<KeyValuePair<string, string>> dictionaryBody)
            {
                httpRequest.Content = new FormUrlEncodedContent(dictionaryBody);
                return httpRequest;
            }

            httpRequest.Content = JsonContent.Create(request.Body);
        }

        return httpRequest;
    }

    protected async Task<IHttpResponse<string>> CreateResponse(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken);

            throw new HttpException()
            {
                Headers = response.Headers,
                StatusCode = response.StatusCode,
                Body = errorBody
            };
        }

        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        var body = bytes?.ToString();

        ArgumentNullException.ThrowIfNull(body);

        return new HttpResponse<string>()
        {
            Body = body,
            Headers = response.Headers,
            StatusCode = response.StatusCode
        };
    }

    protected async Task<IHttpResponse<TResponseBody>> CreateResponse<TResponseBody>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken);

            throw new HttpException()
            {
                Headers = response.Headers,
                StatusCode = response.StatusCode,
                Body = errorBody
            };
        }

        var body = await response.Content.ReadFromJsonAsync<TResponseBody>(cancellationToken);

        ArgumentNullException.ThrowIfNull(body);

        return new HttpResponse<TResponseBody>()
        {
            Body = body,
            Headers = response.Headers,
            StatusCode = response.StatusCode
        };
    }
}