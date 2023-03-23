using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureDeleted(); //uncomment to update or reset database schema, sessions will not be stored in db if uncommented
            Database.EnsureCreated();

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<ActivationCode> ActivationCodes => Set<ActivationCode>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //todo set foreign key orderId for order table 

            modelBuilder.Entity<OrderDetail>() // set composite key for orderdetail
                .HasKey(c => new { c.OrderId, c.ProductId });

            //default database seed values
            modelBuilder.Entity<User>()
                .HasData(
                new User { UserId = 1, Username = "admin", Password = "admin", FirstName = "Jon", LastName = "Snow" },
                new User { UserId = 2, Username = "cherwah", Password = "cherwah", FirstName = "Cher", LastName = "Wah" },
                new User { UserId = 3, Username = "testuser1", Password = "testuser1", FirstName = "Glendon", LastName = "Tan" }); // user with no test data

            modelBuilder.Entity<Product>()
                .HasData(
                new Product { ProductId = "1", ProductName = ".NET Charts", ProductPrice = 99.00, ProductDescription = "Brings powerful capabilities to your .NET applications", ProductImg = "/images/netchart.png" },
                new Product { ProductId = "2", ProductName = ".NET PayPal", ProductPrice = 69.00, ProductDescription = "Integrate your .NET apps with PayPal the easy way", ProductImg = "/images/netpaypal.png" },
                new Product { ProductId = "3", ProductName = ".NET ML", ProductPrice = 299.00, ProductDescription = "Supercharged .NET machine learning libraries", ProductImg = "/images/netml.png" },
                new Product { ProductId = "4", ProductName = ".NET Analytics", ProductPrice = 299.00, ProductDescription = "Performs data mining and analytics easily in .NET", ProductImg = "/images/netanalytics.png" },
                new Product { ProductId = "5", ProductName = ".NET Logger", ProductPrice = 49.00, ProductDescription = "Logs and aggregates events easily in your .NET apps", ProductImg = "/images/netlogger.png" },
                new Product { ProductId = "6", ProductName = ".NET Numerics", ProductPrice = 199.00, ProductDescription = "Powerful numerical methods for your .NET simulations", ProductImg = "/images/netnumerics.png" });

            //Test data
            modelBuilder.Entity<Order>()
                .HasData(
                new Order { UserId = 1, OrderId = "1", Timestamp = new DateTime(2022, 9, 23, 10, 00, 00) },
                new Order { UserId = 1, OrderId = "2", Timestamp = new DateTime(2022, 9, 9, 10, 00, 00) },
                new Order { UserId = 1, OrderId = "3", Timestamp = new DateTime(2022, 9, 15, 10, 00, 00) },
                new Order { UserId = 2, OrderId = "4", Timestamp = new DateTime(2022, 9, 11, 10, 00, 00) },
                new Order { UserId = 2, OrderId = "5", Timestamp = new DateTime(2022, 9, 11, 10, 00, 00) });

            modelBuilder.Entity<OrderDetail>()
                .HasData(
                new OrderDetail { OrderId = "1", ProductId = "1", Quantity = 2, ProductPrice = 99.00, Rating = 2 },
                new OrderDetail { OrderId = "1", ProductId = "2", Quantity = 3, ProductPrice = 69.00, Rating = 3 },
                new OrderDetail { OrderId = "1", ProductId = "3", Quantity = 4, ProductPrice = 299.00, Rating = 4 },
                new OrderDetail { OrderId = "2", ProductId = "4", Quantity = 1, ProductPrice = 299.00 },
                new OrderDetail { OrderId = "2", ProductId = "5", Quantity = 1, ProductPrice = 49.00 },
                new OrderDetail { OrderId = "3", ProductId = "6", Quantity = 1, ProductPrice = 199.00 },
                new OrderDetail { OrderId = "4", ProductId = "2", Quantity = 1, ProductPrice = 69.00 },
                new OrderDetail { OrderId = "5", ProductId = "1", Quantity = 1, ProductPrice = 99.00 });

            modelBuilder.Entity<ActivationCode>()
                .HasData(
                new ActivationCode { Code = "test111", OrderId = "1", ProductId = "1" },
                new ActivationCode { Code = "test112", OrderId = "1", ProductId = "1" },
                new ActivationCode { Code = "test121", OrderId = "1", ProductId = "2" },
                new ActivationCode { Code = "test122", OrderId = "1", ProductId = "2" },
                new ActivationCode { Code = "test123", OrderId = "1", ProductId = "2" },
                new ActivationCode { Code = "test131", OrderId = "1", ProductId = "3" },
                new ActivationCode { Code = "test132", OrderId = "1", ProductId = "3" },
                new ActivationCode { Code = "test133", OrderId = "1", ProductId = "3" },
                new ActivationCode { Code = "test134", OrderId = "1", ProductId = "3" },
                new ActivationCode { Code = "test241", OrderId = "2", ProductId = "4" },
                new ActivationCode { Code = "test251", OrderId = "2", ProductId = "5" },
                new ActivationCode { Code = "test351", OrderId = "3", ProductId = "6" },
                new ActivationCode { Code = "test451", OrderId = "4", ProductId = "2" },
                new ActivationCode { Code = "test551", OrderId = "5", ProductId = "1" }
                );
        }
    }
}
