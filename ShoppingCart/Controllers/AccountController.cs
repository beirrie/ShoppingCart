using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccessLayer;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext db;
        private readonly AccountService _accountService;

        public AccountController(AppDbContext db, AccountService accountService)
        {
            this.db = db;
            _accountService = accountService;
        }

        public IActionResult Index(string returnUrl)
        {   
            if (!_accountService.Authenticate())
            {
                ViewBag.ReturnUrl = returnUrl;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(User user, string returnUrl)
        {
            System.Diagnostics.Debug.WriteLine("Overhere!");
            System.Diagnostics.Debug.WriteLine(returnUrl);
            string username = user.Username;
            ViewData["username"] = username;

            var getUser = db.Users.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();

            if (getUser != null)
            {
                Session session = new Session() //create session object
                {
                    SessionId = Guid.NewGuid().ToString(),
                    UserId = getUser.UserId
                };

                HttpContext.Session.SetString("SessionId", session.SessionId); //set session cookies
                HttpContext.Session.SetInt32("UserId", session.UserId);
                HttpContext.Session.SetString("Name", getUser.FirstName + " " + getUser.LastName);

                var getSession = db.Sessions.Where(x => x.UserId.Equals(getUser.UserId)).FirstOrDefault();

                if (getSession == null) //add session to db
                {
                    db.Sessions.Add(session);
                }
                else // replace old session if found
                {
                    db.Sessions.Remove(getSession);
                    db.Sessions.Add(session);
                }

                db.SaveChanges();

                if (returnUrl != null && !returnUrl.Contains("Account"))
                {
                    return Redirect(returnUrl);

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                //validation error message, empty fields validation handled by view
                TempData["loginflag"] = "Invalid username or password"; //username & password pair not found in db
                return RedirectToAction("Index", "Account");
            }
        }

        public IActionResult Logout()
        {
            string? sessionId = HttpContext.Session.GetString("SessionId");

            Session? session = db.Sessions.Where(d => d.SessionId == sessionId).FirstOrDefault();

            if (session != null) // remove session from db
            {
                db.Sessions.Remove(session);
                db.SaveChanges();
            }

            HttpContext.Session.Clear(); // remove session cookies
            return RedirectToAction("Index", "Account");
        }
    }
}
