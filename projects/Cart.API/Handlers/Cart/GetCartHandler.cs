using AutoMapper;
using Cart.API.Command.Cart;
using Cart.API.Responses.Cart;
using Cart.API.Services;
using Domain.Repositories;
using MediatR;

namespace Cart.API.Handlers.Cart;

public class GetCartHandler : IRequestHandler<GetCartCommand, CartExtendedResponse?>
{
    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICatalogService _catalogService;

    public GetCartHandler(
        ICartRepository repository,
        IMapper mapper,
        ICatalogService catalogService
    )
    {
        _repository = repository;
        _mapper = mapper;
        _catalogService = catalogService;
    }

    public async Task<CartExtendedResponse?> Handle(GetCartCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAsync(request.Id);

        if (result is null)
        {
            return null;
        }

        var extendedResponse = _mapper.Map<CartExtendedResponse>(result);

        if (extendedResponse.Items is not null)
        {
            var tasks = extendedResponse.Items
                .Select(async x => await _catalogService.EnrichCartItem(x, cancellationToken));

            extendedResponse.Items = await Task.WhenAll(tasks);
        }

        return extendedResponse;
    }
}
