using System.Net;
using API.Filters;
using API.ResponseModels;
using Domain.Requests.Item;
using Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RiskFirst.Hateoas;

namespace API.Controllers;

[ApiController]
[Route("api/v1/hateos/items")]
public class ItemHateoasController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly ILinksService _linkService;

    public ItemHateoasController(IItemService itemService, ILinksService linkService)
    {
        _itemService = itemService;
        _linkService = linkService;
    }

    [HttpGet(Name = nameof(Get))]
    public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        var result = await _itemService.GetItemsAsync();

        var totalItems = result.Count();

        var itemsOnPage = result.OrderBy(c => c.Name).Skip(pageIndex * pageSize).Take(pageSize);

        var heteoasResults = new List<ItemHateoasResponse>();

        foreach (var itemResponse in itemsOnPage)
        {
            var heteoasResult = new ItemHateoasResponse()
            {
                Data = itemResponse
            };

            await _linkService.AddLinksAsync(heteoasResult);

            heteoasResults.Add(heteoasResult);
        }

        var model = new PaginatedItemsResponseModel<ItemHateoasResponse>(
            pageIndex,
            pageSize,
            totalItems,
            heteoasResults);

        return Ok(model);
    }

    [HttpGet("{id:guid}", Name = nameof(GetById))]
    [ItemExists]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _itemService.GetItemAsync(new GetItemRequest()
        {
            Id = id
        });

        var heteoasResult = new ItemHateoasResponse()
        {
            Data = result
        };

        await _linkService.AddLinksAsync(heteoasResult);

        return Ok(heteoasResult);
    }

    [HttpPost(Name = nameof(Post))]
    public async Task<IActionResult> Post(AddItemRequest request, [FromServices] IValidator<AddItemRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var response = ValidationErrorResponseFactory.Create(validationResult);
            return BadRequest(response);
        }

        var result = await _itemService.AddItemAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
    }

    [HttpPut("{id:guid}", Name = nameof(Put))]
    [ItemExists]
    public async Task<IActionResult> Put(Guid id, EditItemRequest request)
    {
        request.Id = id;
        var result = await _itemService.EditItemAsync(request);
        var hateoasResult = new ItemHateoasResponse { Data = result };
        await _linkService.AddLinksAsync(hateoasResult);
        return Ok(hateoasResult);
    }

    [HttpDelete("{id:guid}", Name = nameof(Delete))]
    [ItemExists]
    public async Task<IActionResult> Delete(Guid id)
    {
        var request = new DeleteItemRequest { Id = id };
        await _itemService.DeleteItemAsync(request);
        return NoContent();
    }
}
