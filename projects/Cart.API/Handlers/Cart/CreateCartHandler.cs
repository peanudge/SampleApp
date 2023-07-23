using AutoMapper;
using Cart.API.Commands.Cart;
using Cart.API.Entities;
using Cart.API.Models;
using Cart.API.Responses.Cart;
using Cart.API.Services;
using Domain.Repositories;
using MediatR;

namespace Domain.Handler.Cart;

public class CreateCartHandler : IRequestHandler<CreateCartCommand, CartExtendedResponse>
{
    private readonly ICartRepository _repository;
    private readonly ICatalogService _catalogService;
    private readonly IMapper _mapper;

    public CreateCartHandler(ICartRepository cartRepository, IMapper mapper, ICatalogService catalogService)
    {
        _repository = cartRepository;
        _catalogService = catalogService;
        _mapper = mapper;
    }

    public async Task<CartExtendedResponse> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var entity = new CartSession()
        {
            Items = command.ItemIds?.Select(x => new CartItem() { CartItemId = new Guid(x), Quantity = 1 }).ToList(),
            User = new CartUser { Email = command.UserEmail },
            ValidityDate = DateTimeOffset.Now.AddMonths(2),
            Id = Guid.NewGuid().ToString()
        };

        var result = await _repository.AddOrUpdateAsync(entity);

        var response = _mapper.Map<CartExtendedResponse>(result);

        if (response.Items is not null)
        {
            var tasks = response.Items.Select(async x => await _catalogService.EnrichCartItem(x, cancellationToken));
            response.Items = await Task.WhenAll(tasks);
        }
        return response;
    }
}
