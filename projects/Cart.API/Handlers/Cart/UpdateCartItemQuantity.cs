using AutoMapper;
using Cart.API.Commands.Cart;
using Cart.API.Responses.Cart;
using Cart.API.Services;
using Domain.Repositories;
using MediatR;

namespace Cart.API.Handlers.Cart;

public class UpdateCartItemQuantity : IRequestHandler<UpdateCartItemQuantityCommand, CartExtendedResponse>
{
    private readonly ICartRepository _repository;
    private readonly ICatalogService _catalogService;
    private readonly IMapper _mapper;

    public UpdateCartItemQuantity(ICartRepository cartRepository, IMapper mapper, ICatalogService catalogService)
    {
        _repository = cartRepository;
        _catalogService = catalogService;
        _mapper = mapper;
    }

    public async Task<CartExtendedResponse> Handle(UpdateCartItemQuantityCommand command, CancellationToken cancellationToken)
    {
        var cartDetail = await _repository.GetAsync(command.CartId);

        if (command.IsAddOperation)
        {
            cartDetail?.Items?.FirstOrDefault(x => x.CartItemId == command.CartItemId)?.IncreaseQuantity();
        }
        else
        {
            cartDetail?.Items?.FirstOrDefault(x => x.CartItemId == command.CartItemId)?.DecreaseQuantity();
        }

        var cartItemsList = cartDetail?.Items?.ToList() ?? new List<Models.CartItem>();

        cartItemsList.RemoveAll(x => x.Quantity == 0);

        cartDetail!.Items = cartItemsList;

        await _repository.AddOrUpdateAsync(cartDetail);

        var response = _mapper.Map<CartExtendedResponse>(cartDetail);

        if (response.Items is not null)
        {
            var tasks = response.Items.Select(async x => await _catalogService.EnrichCartItem(x, cancellationToken));
            response.Items = await Task.WhenAll(tasks);
        }

        return response;
    }
}
