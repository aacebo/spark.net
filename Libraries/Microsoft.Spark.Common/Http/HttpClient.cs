using System.Net.Http.Json;

namespace Microsoft.Spark.Common.Http;

public interface IHttpClient : IDisposable
{
    public Task<IHttpResponse<string>> SendAsync(IHttpRequest request);
    public Task<IHttpResponse<TResponseBody>> SendAsync<TResponseBody>(IHttpRequest request);
    public Task<IHttpResponse<string>> SendAsync(IHttpRequest request, CancellationToken cancellationToken);
    public Task<IHttpResponse<TResponseBody>> SendAsync<TResponseBody>(IHttpRequest request, CancellationToken cancellationToken);
}

public class HttpClient : IHttpClient
{
    protected System.Net.Http.HttpClient _client;
    protected IHttpRequestOptions _requestOptions;

    public HttpClient()
    {
        _client = new System.Net.Http.HttpClient();
        _requestOptions = new HttpRequestOptions();
    }

    public HttpClient(IHttpRequestOptions requestOptions)
    {
        _client = new System.Net.Http.HttpClient();
        _requestOptions = requestOptions;
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

        foreach (var (key, value) in _requestOptions.Headers)
        {
            httpRequest.Headers.Add(key, value);
        }

        foreach (var (key, value) in request.Headers)
        {
            httpRequest.Headers.Add(key, value);
        }

        if (request.Body != null)
        {
            if (request.Body is string body)
            {
                httpRequest.Content = new StringContent(body);
                return httpRequest;
            }

            httpRequest.Content = JsonContent.Create(request.Body);
        }

        return httpRequest;
    }

    protected async Task<IHttpResponse<string>> CreateResponse(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        response.EnsureSuccessStatusCode();
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
        response.EnsureSuccessStatusCode();
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