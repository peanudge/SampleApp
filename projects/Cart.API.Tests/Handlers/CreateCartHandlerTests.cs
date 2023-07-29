using AutoMapper;
using Cart.API.Commands.Cart;
using Cart.API.Mapper;
using Domain.Handler.Cart;

namespace Cart.API.Tests.Hanlders;

public class CreateCartHandlerTests : IClassFixture<CartContextFactory>
{
    private readonly CartContextFactory _cartContextFactory;

    public CreateCartHandlerTests(CartContextFactory cartContextFactory)
    {
        _cartContextFactory = cartContextFactory;
    }

    [Fact]
    public async Task HandleShouldCreateANewRecordAndReturn()
    {
        var handler = new CreateCartHandler(
            _cartContextFactory.GetCartRepository(),
            new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CartProfile>())),
            _cartContextFactory.GetCatalogService()
        );

        var result = await handler.Handle(
            new CreateCartCommand
            {
                ItemIds = new[] { "be05537d-5e80-45c1-bd8c-aa21c0f1251e", "f5da5ce4-091e-492e-a70a-22b073d75a52" },
                UserEmail = "sample123@gmail.com"
            },
            CancellationToken.None);

        Assert.NotNull(result.Id);
        Assert.NotNull(result.Items);
        Assert.NotNull(result.User);
        Assert.NotEqual(default(DateTimeOffset), result.ValidityDate);
    }
}
