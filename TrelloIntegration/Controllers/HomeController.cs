using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrelloIntegration.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            var token = HttpContext.Session["trello_token"];
            if (token == null)
                return View();

            return RedirectToAction("Cards");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(String token)
        {
            if (HttpContext.Session["trello_token"] != null)
                HttpContext.Session.Remove("trello_token");

            HttpContext.Session.Add("trello_token", token);

            return Json(new { ok = true, newurl = Url.Action("Login") });
        }

        [HttpGet]
        public ActionResult Cards()
        {
            var token = HttpContext.Session["trello_token"];
            if (token == null)
                return RedirectToAction("Login");
            return View();
        }

        //No antiforgery token on this for the demo
        //Any cross site calls to logout would be mainly just be annoying.
        [HttpPost]
        public ActionResult Logout()
        {
            HttpContext.Session.Abandon();
            return Json(new { ok = true, newurl = Url.Action("Login") });
        }
    }
}