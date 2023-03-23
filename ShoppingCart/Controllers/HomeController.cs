using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ShoppingCart.DataAccessLayer;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        private readonly ProductRatingService productRatingService;

        public HomeController(AppDbContext db, ProductRatingService productRatingService)
        {
            this.db = db;
            this.productRatingService = productRatingService;
        }

        public IActionResult Index()
        {
            List<Product> products = db.Products.ToList();

            // Get all average rating from order details
            List<ProductRatingViewModel> productRatings = this.productRatingService.GetProductRating();

            // Set Average rating from order details to Product Rating
            productRatingService.SetProductRating(products, productRatings);

            ViewData["products"] = products;
            return View();
        }
  
        [HttpPost]
        public IActionResult Search(string searchString)
        {
            List<Product> products;
            List<ProductRatingViewModel> productRatings = this.productRatingService.GetProductRating();
            if (string.IsNullOrEmpty(searchString))
            {
                products = db.Products.ToList();
                ViewData["searchstring"] = "";
            }
            else
            {
                products = db.Products.Where(x => x.ProductName.ToLower().Contains(searchString.Trim().ToLower()) ||
                    x.ProductDescription.ToLower().Contains(searchString.Trim().ToLower())).ToList();
                ViewData["searchstring"] = searchString;
            }
            productRatingService.SetProductRating(products, productRatings);

            ViewData["products"] = products;

            return View("Index");
        }
    }
}