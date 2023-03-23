namespace ShoppingCart.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual User user { get; set; } = null!;
        public virtual ICollection<ActivationCode> ActivationCodes { get; set; } = new HashSet<ActivationCode>();
        public Order()
        {
            OrderId = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
            OrderDetails = new HashSet<OrderDetail>();
        }
    }
}
