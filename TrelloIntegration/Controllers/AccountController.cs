using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrelloIntegration.DAL;
using TrelloIntegration.Services;

namespace TrelloIntegration.Controllers
{
    public class AccountController : Controller
    {
        private const string TRELLO_USER_COOKIE_NAME = "TrelloIntegrationUser";
        private ITrelloIntegrationContext _db = new TrelloIntegrationContext();

        public AccountController()
        {
            _db = new TrelloIntegrationContext();
        }

        public AccountController(ITrelloIntegrationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Login()
        {
            var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

            if (userCookie == null)
                return View();

            var userID = userCookie.Value;

            if (string.IsNullOrWhiteSpace(userCookie.Value))
                return View();

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string token)
        {
            var user = await new TrelloService().GetMemberForUserToken(token);

            if (user == null)
                return Json(new { ok = false });

            user.TrelloToken = token;

            HttpContext.Response.Cookies.Add(new HttpCookie(TRELLO_USER_COOKIE_NAME)
            {
                Value = user.ID,
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(3)
            });

            var storedUser = _db.Users.FirstOrDefault(u => u.ID == user.ID);
            if (storedUser != null)
            {
                storedUser.TrelloToken = user.TrelloToken;
                storedUser.FullName = user.FullName;
                storedUser.LastLoggedIn = DateTime.Now;
            }
            else
            {
                user.LastLoggedIn = DateTime.Now;
                _db.Users.Add(user);
            }

           _db.SaveChanges();

            return Json(new { ok = true, newurl = Url.Action("Login") });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

            if (userCookie == null)
                return Json(new { ok = true, newurl = Url.Action("Login") });

            //Expire user cookie
            Response.Cookies.Add(new HttpCookie(TRELLO_USER_COOKIE_NAME)
            {
                Expires = DateTime.Now.AddDays(-1)
            });

            return Json(new { ok = true, newurl = Url.Action("Login") });
        }
    }
}