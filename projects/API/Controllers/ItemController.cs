using Domain.Requests.Item;
using Domain.Responses.Item;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ItemResponse>))]
    public async Task<IActionResult> Get()
    {
        var result = await _itemService.GetItemsAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
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


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Post([FromBody] AddItemRequest request)
    {
        var result = await _itemService.AddItemAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemResponse))]
    public async Task<IActionResult> Put(Guid id, [FromBody] EditItemRequest request)
    {
        request.Id = id;
        var result = await _itemService.EditItemAsync(request);
        return Ok(result);
    }
}
