using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrelloIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(String token)
        {
            


            return Json(new { ok = true, newurl = Url.Action("Index") });
            //test
        }
    }
}