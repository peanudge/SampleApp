using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.Genre
{
    public class GetGenreRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
