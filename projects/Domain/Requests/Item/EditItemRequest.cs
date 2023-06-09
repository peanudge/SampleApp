using Domain.Models;

namespace Domain.Requests.Item;

public class EditItemRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string LabelName { get; set; } = null!;
    public Price Price { get; set; } = null!;
    public string PictureUri { get; set; } = null!;
    public DateTimeOffset ReleaseDate { get; set; }
    public string Format { get; set; } = null!;
    public int AvailableStock { get; set; }
    public Guid GenreId { get; set; }
    public Guid ArtistId { get; set; }
}