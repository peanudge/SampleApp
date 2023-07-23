namespace API.Client.Base;

public interface IBaseClient
{
    Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken);
    Uri BuildUri(string format);
}
