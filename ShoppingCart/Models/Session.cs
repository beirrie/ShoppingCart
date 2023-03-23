using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Session
    {
        public string SessionId { get; set; } = "";
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public Session()
        {
            Timestamp = DateTime.Now;
        }
    }

}
