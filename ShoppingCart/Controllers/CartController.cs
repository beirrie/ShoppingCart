using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ShoppingCart.DataAccessLayer;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly AccountService _accountService;
        private readonly AppDbContext db;

        public CartController(AppDbContext db, AccountService accountService)
        {
            _accountService = accountService;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IEnumerable<ProductJsonModel> Index([FromBody] ProductRequestModel[] cartItems)
        {
            List<ProductJsonModel> result = new List<ProductJsonModel>();
            List<Product> productListing = new List<Product>();
            // list of product ids in localStorage.
            List<string> cartProductIds = new List<string>();
            if (cartItems != null)
            {
                foreach (var item in cartItems)
                {
                    cartProductIds.Add(item.Id);
                }
                var cartProducts = db.Products.Where(p => cartProductIds.Contains(p.ProductId)).ToList();

                foreach (var product in cartProducts)
                {
                    result.Add(new ProductJsonModel()
                    {
                        ProductDescription = product.ProductDescription,
                        ProductId = product.ProductId,
                        ProductImg = product.ProductImg,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice
                    });
                }
            }
            return result;
        }

        [HttpPost]
        public bool Checkout([FromBody] ProductRequestModel[] checkoutCart)
        {
    
            if (_accountService.Authenticate() && checkoutCart.Any())
            {
                var userSession = db.Sessions.FirstOrDefault(x => x.SessionId == HttpContext.Session.GetString("SessionId"));
                if (userSession != null)
                {
                    var userId = userSession.UserId;
                    List<string> checkoutCartIds = new List<string>();
                    foreach (var p in checkoutCart)
                    {
                        checkoutCartIds.Add(p.Id);
                    }
                    string OrderId = Guid.NewGuid().ToString();
                    for (int i = 0; i < checkoutCart.Length; i++)
                    {
                        var product = db.Products.FirstOrDefault(p => p.ProductId == checkoutCart[i].Id);
                        if (product != null)
                        {
                            double productPrice = product.ProductPrice;

                            OrderDetail orderDetail = new OrderDetail
                            {
                                OrderId = OrderId,
                                ProductId = checkoutCart[i].Id,
                                Quantity = checkoutCart[i].Count,
                                ProductPrice = productPrice
                            };
                            db.OrderDetails.Add(orderDetail);

                            for (int j = 0; j < checkoutCart[i].Count; j++)
                            {
                                ActivationCode activationCode = new ActivationCode
                                {
                                    Code = Guid.NewGuid().ToString(),
                                    ProductId = checkoutCart[i].Id,
                                    OrderId = orderDetail.OrderId
                                };
                                db.ActivationCodes.Add(activationCode);
                            }
                        }
                    }
                    db.Orders.Add(new Order
                    {
                        OrderId = OrderId,
                        UserId = userId,
                        Timestamp = DateTime.Now
                    });
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
