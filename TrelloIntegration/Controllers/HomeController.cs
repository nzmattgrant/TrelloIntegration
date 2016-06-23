using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrelloIntegration.Services;
using TrelloIntegration.ViewModels;

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

        //TODO rename to dashboard
        //Set up a partial method so that we are not taking forever hitting the trello api
        [HttpGet]
        public async Task<ActionResult> Cards()
        {
            var token = HttpContext.Session["trello_token"];
            if (token == null)
                return RedirectToAction("Login");

            var dashboardViewModel = new DashboardViewModel();
            await dashboardViewModel.SetUp(token.ToString());

            return View(dashboardViewModel);
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