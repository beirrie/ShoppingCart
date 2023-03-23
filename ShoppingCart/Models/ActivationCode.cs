using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class ActivationCode
    {
        [Key]
        public string Code { get; set; }
        public string ProductId { get; set; } = "";
        public string OrderId { get; set; } = "";
        public virtual Product Product { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;

        public ActivationCode()
        {
            Code = Guid.NewGuid().ToString();
        }
    }
}
