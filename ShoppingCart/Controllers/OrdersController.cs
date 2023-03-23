using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ShoppingCart.DataAccessLayer;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext db;
        private readonly AccountService _accountService;

        public OrdersController(AppDbContext db, AccountService accountService)
        {
            _accountService = accountService;
            this.db = db;
        }

        public IActionResult Index()
        {
            if (!_accountService.Authenticate())
            {
                string returnUrl = HttpContext.Request.Path.Value.ToString();
                return RedirectToAction("Index", "Account", new { returnUrl });
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            var orders = db.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.Timestamp).ToList();
            List<PurchaseViewModel> purchases = new List<PurchaseViewModel>();
            foreach (Order order in orders)
            {
                var orderDetails = db.OrderDetails.Include(x => x.Product).OrderBy(x => x.Product.ProductName).Where(od => od.OrderId == order.OrderId && od.ProductId == od.Product.ProductId).ToList();
                foreach (var detail in orderDetails)
                {
                    PurchaseViewModel purchase = new PurchaseViewModel();
                    purchase.OrderId = detail.OrderId;
                    purchase.Date = order.Timestamp;
                    purchase.ProductId = detail.ProductId;
                    purchase.PurchasePrice = detail.ProductPrice;
                    purchase.Quantity = detail.Quantity;
                    purchase.Rating = detail.Rating;
                    purchase.ProductName = detail.Product.ProductName;
                    purchase.ProductDescription = detail.Product.ProductDescription;
                    purchase.ProductImg = detail.Product.ProductImg;
                    purchase.ActivationCodes = db.ActivationCodes.Where(ac => ac.ProductId == detail.ProductId && ac.OrderId == detail.OrderId).ToList();
                    purchases.Add(purchase);
                }
            }
            return View(purchases);
        }

        [HttpPost]
        public IActionResult UpdateRating([FromBody] OrderDetail updatedOrderDetails)
        {
            OrderDetail orderdetail = db.OrderDetails.FirstOrDefault(x => x.OrderId == updatedOrderDetails.OrderId && x.ProductId == updatedOrderDetails.ProductId);
            if (orderdetail != null)
            {
                orderdetail.Rating = updatedOrderDetails.Rating;
                db.SaveChanges();
                Debug.WriteLine("successful");
            }
            return Json(new { updatedOrderDetails });
        }
    }
}
