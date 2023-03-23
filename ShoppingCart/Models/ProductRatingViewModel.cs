namespace ShoppingCart.Models
{
    public class ProductRatingViewModel
    {
        public string ProductId { get; set; } = "";
        public double Rating { get; set; }
        public int RatingInt
        {
            get
            {
                return (int)Rating;
            }
        }
    }
}
