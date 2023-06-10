using Domain.Models;
using Domain.Requests.Item;
using Domain.Responses.Item;

namespace Domain.Mappers;

public interface IItemMapper
{
    Item Map(AddItemRequest item);
    Item Map(EditItemRequest item);
    ItemResponse Map(Item item);
}