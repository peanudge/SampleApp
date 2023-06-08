namespace Domain.Models
{
    public class Genre
    {
        public Guid GenreId { get; set; }
        public string GenreDescription { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = null!;
    }
}