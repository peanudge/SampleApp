namespace Domain.Models
{
    public class Price
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
    }
}