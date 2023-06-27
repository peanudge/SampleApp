using API.Filters;
using Domain.Requests.Item;
using Domain.Responses.Item;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace API.Tests.Filter;


public class ItemExistsAttributeTests
{
    [Fact]
    public async Task ShouldContinuePipelineWhenIdIsPresent()
    {
        // Given
        var id = Guid.NewGuid();
        var itemService = new Mock<IItemService>();

        itemService
            .Setup(x => x.GetItemAsync(It.IsAny<GetItemRequest>()))
            .ReturnsAsync(new ItemResponse { Id = id });

        var filter = new ItemExistsAttribute.ItemExistsFilterImpl(itemService.Object);

        var actionExecuteContext = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(){
                {"id", id}
            }, new object());

        var nextCallback = new Mock<ActionExecutionDelegate>();

        // When
        await filter.OnActionExecutionAsync(actionExecuteContext, nextCallback.Object);

        // Then
        nextCallback.Verify(executionDelegate => executionDelegate(), Times.Once);
    }
}
