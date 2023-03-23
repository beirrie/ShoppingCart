using System.ComponentModel.DataAnnotations.Schema;
namespace ShoppingCart.Models
{
    public class Product
    {
        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; } = "";
        public string ProductImg { get; set; } = "";
        [NotMapped]
        public int AverageRatingFromOrderDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public virtual ICollection<ActivationCode> ActivationCodes { get; set; } = new HashSet<ActivationCode>();
    }
}