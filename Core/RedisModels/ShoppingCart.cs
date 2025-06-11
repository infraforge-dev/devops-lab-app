namespace Core.RedisModels
{
    public class ShoppingCart
    {
        public string? CartId { get; set; }

        public List<CartItem> Items { get; set; } = [];
    }
}
