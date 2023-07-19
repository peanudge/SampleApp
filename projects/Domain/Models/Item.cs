namespace Domain.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string LabelName { get; set; } = null!;
        public string PictureUri { get; set; } = null!;
        public DateTimeOffset ReleaseDate { get; set; }
        public string Format { get; set; } = null!;
        public int AvailableStock { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
        public Price Price { get; set; } = null!;
        public bool IsInactive { get; set; }
    }
}
