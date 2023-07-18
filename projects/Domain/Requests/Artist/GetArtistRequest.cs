using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.Artist
{
    public class GetArtistRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
