using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class OrderDetail
    {
        public string OrderId { get; set; } = "";
        public string ProductId { get; set; } = "";
        public int Quantity { get; set; }
        public double ProductPrice { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; } = 5;
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public OrderDetail()
        {
        }
    }
}
