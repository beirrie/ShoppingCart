using ShoppingCart.DataAccessLayer;

namespace ShoppingCart.Services
{
    public class AccountService
    {
        private readonly AppDbContext db;
        private readonly IHttpContextAccessor contextAccessor;

        public AccountService(AppDbContext db, IHttpContextAccessor contextAccessor)
        {
            this.db = db;
            this.contextAccessor = contextAccessor;
        }
        public bool Authenticate()
        {
            bool isAuth = false;

            string sessionId;
            int userId;

            try
            {
                sessionId = contextAccessor.HttpContext.Session.GetString("SessionId").ToString();
                userId = (int)contextAccessor.HttpContext.Session.GetInt32("UserId");
            }
            catch (NullReferenceException)
            {
                return isAuth;
            }

            var session = db.Sessions.Where(d => d.SessionId == sessionId && d.UserId == userId).FirstOrDefault();

            if (session != null)
            {
                isAuth = true;
            }
            return isAuth;
        }
    }
}
