using Domain.Models;
using Domain.Requests.Item;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/items")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Item>))]
    public async Task<IActionResult> Get()
    {
        var result = await _itemService.GetItemsAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await _itemService.GetItemAsync(new GetItemRequest
        {
            Id = id
        });

        if (result == null)
        {
            return Problem(title: "Can not found",
                        detail: $"Item({id}) is not found.",
                        statusCode: StatusCodes.Status400BadRequest);
        }

        return Ok(result);
    }
}