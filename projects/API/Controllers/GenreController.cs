using API.ResponseModels;
using Domain.Requests.Genre;
using Domain.Responses.Item;
using Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/genre")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int pageSize = 0, [FromQuery] int pageIndex = 0)
    {
        var result = await _genreService.GetGenresAsync();
        var totalItems = result.ToList().Count;
        var itemsOnPage = result.OrderBy(c => c.GenreDescription)
            .Skip(pageSize * pageIndex)
            .Take(pageSize);

        var model = new PaginatedItemsResponseModel<GenreResponse>(pageIndex, pageSize, totalItems, itemsOnPage);
        return Ok(model);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _genreService.GetGenreAsync(new GetGenreRequest { Id = id });
        return Ok(result);
    }

    [HttpGet("{id:guid}/items")]
    public async Task<IActionResult> GetItemById(Guid id)
    {
        var result = await _genreService.GetItemsByGenreIdAsync(new GetGenreRequest { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        AddGenreRequest request,
        [FromServices] IValidator<AddGenreRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var response = ValidationErrorResponseFactory.Create(validationResult);
            return BadRequest(response);
        }

        var result = await _genreService.AddGenreAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.GenreId }, null);
    }
}
