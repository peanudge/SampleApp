using MediatR;

namespace Cart.API.Events;

public class ItemSoldOutEvent : IRequest<Unit>
{
    public string? Id { get; set; }
}
