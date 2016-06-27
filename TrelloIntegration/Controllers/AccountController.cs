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
        private ITrelloIntegrationContext _db { get; set; }
        private ITrelloService _service { get; set; }

        public AccountController()
        {
            _db = new TrelloIntegrationContext();
            _service = new TrelloService();
        }

        public AccountController(ITrelloIntegrationContext db, ITrelloService service)
        {
            _db = db;
            _service = service;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string token)
        {
            var user = await _service.GetMemberForUserToken(token);

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

            return Json(new { ok = true, newurl = Url.Action("Index", "Dashboard", null) });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            //Expire user cookie
            Response.Cookies.Add(new HttpCookie(TRELLO_USER_COOKIE_NAME)
            {
                Expires = DateTime.Now.AddDays(-1)
            });

            return Json(new { ok = true, newurl = Url.Action("Login") });
        }
    }
}