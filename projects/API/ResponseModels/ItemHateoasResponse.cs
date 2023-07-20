using Domain.Responses.Item;
using Newtonsoft.Json;
using RiskFirst.Hateoas.Models;

namespace API.ResponseModels;

public class ItemHateoasResponse : ILinkContainer
{
    public ItemResponse? Data;
    private Dictionary<string, Link>? _links;

    [JsonProperty(PropertyName = "_links")]
    public Dictionary<string, Link> Links
    {
        get => _links ?? (_links = new Dictionary<string, Link>());
        set => _links = value;
    }
    public void AddLink(string id, Link link)
    {
        Links.Add(id, link);
    }
}
