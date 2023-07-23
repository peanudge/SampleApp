namespace API.Client;
using API.Client.Resources;


public interface ICatalogClient
{
    ICatalogItemResource Item { get; }
}
