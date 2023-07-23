namespace Cart.API.Models
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }
        public int Quantity { get; set; }

        public void IncreaseQuantity(int quantity = 1)
        {
            Quantity += quantity;
        }

        public void DecreaseQuantity(int quantity = 1)
        {
            Quantity -= quantity;
        }
    }
}
