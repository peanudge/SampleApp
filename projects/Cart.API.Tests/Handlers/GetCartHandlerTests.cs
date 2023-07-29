using AutoMapper;
using Cart.API.Command.Cart;
using Cart.API.Entities;
using Cart.API.Handlers.Cart;
using Cart.API.Mapper;
using Cart.API.Responses.Cart;

namespace Cart.API.Tests;

public class GetCartHandlerTests : IClassFixture<CartContextFactory>
{
    private CartContextFactory _cartContextFactory;

    public GetCartHandlerTests(CartContextFactory cartContextFactory)
    {
        _cartContextFactory = cartContextFactory;
    }

    [Fact]
    public async Task HandleShouldRetrieveANewRecordAndReturnIt()
    {
        var handler = new GetCartHandler(
            _cartContextFactory.GetCartRepository(),
            new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CartProfile>())),
            _cartContextFactory.GetCatalogService());

        var result = await handler.Handle(
            new GetCartCommand
            {
                Id = new Guid("9ced6bfa-9659-462e-aece-49fe50613e96")
            },
            CancellationToken.None
        );

        var cartResponse = Assert.IsType<CartExtendedResponse>(result);
        Assert.NotNull(cartResponse.Id);
        Assert.NotNull(cartResponse.Items);
        Assert.NotNull(cartResponse.User);

        Assert.Equal(DateTimeOffset.MinValue, cartResponse.ValidityDate); // TODO: ValidityDate is not implemented
    }
}
