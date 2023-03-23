using System.ComponentModel;

namespace ShoppingCart.Models
{
    public class PurchaseViewModel
    {
        public string OrderId { get; set; } = "";
        public DateTime Date { get; set; }
        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public string ProductDescription { get; set; } = "";
        public string ProductImg { get; set; } = "";
        public int Quantity { get; set; }
        public double PurchasePrice { get; set; }
        public int Rating { get; set; }
        public List<ActivationCode> ActivationCodes { get; set; } = new List<ActivationCode>();
    }
}
