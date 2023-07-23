using Newtonsoft.Json;

namespace API.Client.Base;

public class BaseClient : IBaseClient
{
    private readonly HttpClient _client;
    private readonly string _baseUri;

    public BaseClient(HttpClient client, string baseUri)
    {
        _client = client;
        _baseUri = baseUri;
    }

    public Uri BuildUri(string format)
    {
        return new UriBuilder(_baseUri)
        {
            Path = format
        }.Uri;
    }

    public async Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken)
    {
        var result = await _client.GetAsync(uri, cancellationToken);
        result.EnsureSuccessStatusCode();
        var content = await result.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content) ?? throw new InvalidOperationException("Deserialization failed.");
    }
}
