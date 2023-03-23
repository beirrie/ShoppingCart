using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ShoppingCart.Models;
using ShoppingCart.DataAccessLayer;

namespace ShoppingCart.Services
{
    public class ProductRatingService
    {
        private readonly AppDbContext db;

        public ProductRatingService(AppDbContext db)
        {
            this.db = db;
        }

        public List<ProductRatingViewModel> GetProductRating()
        {
            IEnumerable<ProductRatingViewModel> productRatingViewModels = from r in db.OrderDetails.AsNoTracking()
                                                                          group r by r.ProductId into g
                                                                          select new ProductRatingViewModel
                                                                          {
                                                                              ProductId = g.Key,
                                                                              Rating = g.Average(x => x.Rating)
                                                                          };
            return productRatingViewModels.ToList();
        }

        public void SetProductRating(List<Product> products, List<ProductRatingViewModel> productRatings)
        {
            foreach (var product in products)
            {
                var productRating = productRatings.FirstOrDefault(x => x.ProductId == product.ProductId);
                if (productRating != null)
                    product.AverageRatingFromOrderDetails = productRating.RatingInt;
            }
        }
    }
}
