using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrelloIntegration.DAL;
using TrelloIntegration.Services;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Controllers
{
    public class DashboardController : Controller
    {
        private TrelloService _service { get; set; }
        private TrelloIntegrationContext _db = new TrelloIntegrationContext();
        public const string TRELLO_USER_COOKIE_NAME = "TrelloIntegrationUser";
        //private string _trelloAuthToken = null;
        //private string _memberId = null;

        //private void ensureTokenIsSet()
        //{
        //    //TODO i'm not sure if this is a good place to get the cookie
        //    var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

        //    if (userCookie == null || userCookie.Value == null)
        //        _service = null;

        //    var userID = userCookie.Value;

        //    var user = _db.Users.FirstOrDefault(u => u.id == userID);

        //    if (user == null)
        //        _service = null;

        //    _service = new TrelloService(user.TrelloToken);

        //    _trelloAuthToken = user.TrelloToken;
        //    _memberId = user.id;
        //}

        public async Task<ActionResult> Index()
        {
            var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

            if (userCookie == null || userCookie.Value == null)
                return RedirectToAction("Login", "Account");

            var userID = userCookie.Value;

            var user = _db.Users.FirstOrDefault(u => u.id == userID);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var dashboardViewModel = new DashboardViewModel();
            await dashboardViewModel.SetUp(new TrelloService(user.TrelloToken), user.id);
            return View(dashboardViewModel);
        }

        public async Task<ActionResult> Board(string boardID)
        {
            var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

            if (userCookie == null || userCookie.Value == null)
                return RedirectToAction("Login", "Account");

            var userID = userCookie.Value;

            var user = _db.Users.FirstOrDefault(u => u.id == userID);

            if (user == null)
                return RedirectToAction("Login", "Account");

            _service = new TrelloService(user.TrelloToken);

            var board = await _service.GetBoard(boardID);
            var cards = await _service.GetCardsForBoard(boardID);
            var lists = await _service.GetListsForBoard(boardID);

            foreach(var list in lists)
            {
                list.Cards = cards.Where(c => c.idList == list.id);
            }

            board.Lists = lists;

            return View(board);
        }

        public async Task<ActionResult> Card(string cardID)
        {
            var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

            if (userCookie == null || userCookie.Value == null)
                return RedirectToAction("Login", "Account");

            var userID = userCookie.Value;

            var user = _db.Users.FirstOrDefault(u => u.id == userID);

            if (user == null)
                return RedirectToAction("Login", "Account");

            _service = new TrelloService(user.TrelloToken);
            var card = await _service.GetCard(cardID);
            var comments = await _service.GetCommentsForCard(cardID);

            card.Comments = comments;

            return View(card);
        }
    }
}