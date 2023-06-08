namespace Domain.Models
{
    public class Artist
    {
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = null!;
    }
}