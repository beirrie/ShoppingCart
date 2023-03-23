namespace ShoppingCart.Models
{
    public sealed class ProductJsonModel
    {
        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; } = "";
        public string ProductImg { get; set; } = "";
    }
}
