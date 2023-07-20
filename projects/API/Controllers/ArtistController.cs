using System.Net;
using API.ResponseModels;
using Domain.Requests.Artist;
using Domain.Responses.Item;
using Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/v1/artist")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        // TODO: Improve pagination using adhoc query
        var result = await _artistService.GetArtistsAsync();
        var totalItems = result.ToList().Count;
        var itemsOnPage = result
            .OrderBy(c => c.ArtistName)
            .Skip(pageSize * pageIndex)
            .Take(pageSize);

        var model = new PaginatedItemsResponseModel<ArtistResponse>(pageIndex, pageSize, totalItems, itemsOnPage);
        return Ok(model);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _artistService.GetArtistAsync(new GetArtistRequest()
        {
            Id = id
        });

        if (result == null)
        {
            return Problem("Artist not found", statusCode: (int)HttpStatusCode.BadRequest);
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/items")]
    public async Task<IActionResult> GetItemsbyId(Guid id)
    {
        var result = await _artistService.GetItemsByArtistIdAsync(new GetArtistRequest { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddArtistRequest request, [FromServices] IValidator<AddArtistRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var response = ValidationErrorResponseFactory.Create(validationResult);
            return BadRequest(response);
        }

        var result = await _artistService.AddArtistAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.ArtistId }, null);
    }
}
