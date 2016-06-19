using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrelloIntegration.Models;

namespace TrelloIntegration.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var token = HttpContext.Session["trello_token"];
            if (token == null)
                return View();

            return RedirectToAction("Cards");
            //If we have the token matched to the session then we want to retreive it and redirect to the editing view
            //If not then redirect to the login page
        }

        //TODO add in the verification token
        [HttpPost]
        public ActionResult Index(String token)
        {
            //Check if the token is valid for the application
            //Store the token on the session
            //If the token exists already then this session had a valid token at some point so return an error
            if (HttpContext.Session["trello_token"] != null)
                HttpContext.Session.Remove("trello_token");

            HttpContext.Session.Add("trello_token", token);


            //var sessionID = HttpContext.Session.SessionID;
            //var sessionInfo = new SessionInfo { SessionID = sessionID, UserToken = token };
            //var context = new TrelloIntegrationContext();
            //context.SessionInfos.Add(sessionInfo);
            //context.SaveChanges();
            //Check if the token is there and not null
            //If it is good to go then we will store it in the database
            //Then return
            //If there is an error then return an error message

            return Json(new { ok = true, newurl = Url.Action("Index") });
        }

        [HttpGet]
        public ActionResult Cards()
        {
            var token = HttpContext.Session["trello_token"];
            if (token == null)
                return RedirectToAction("Index");
            //Set up a view model with the token and any other shizz
            return View();
        }


        [HttpPost]
        public ActionResult Logout()
        {
            HttpContext.Session.Abandon();
            return Json(new { ok = true, newurl = Url.Action("Index") });
        }
    }
}