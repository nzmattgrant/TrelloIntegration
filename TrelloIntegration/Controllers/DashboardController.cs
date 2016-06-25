using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrelloIntegration.DAL;
using TrelloIntegration.Models;
using TrelloIntegration.Services;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Controllers
{
    public class DashboardController : Controller
    {
        private const string TRELLO_USER_COOKIE_NAME = "TrelloIntegrationUser";
        private TrelloIntegrationContext _db = new TrelloIntegrationContext();

        //These are set and checked with the user's info from the user cookie on each action call
        private TrelloService _service = null;
        private bool _userInformationMissing = false;
        private string _trelloMemberID = null;

        public async Task<ActionResult> Index()
        {
            if (_userInformationMissing)
                return RedirectToAction("Login", "Account");

            var dashboardViewModel = new DashboardViewModel();
            await dashboardViewModel.SetUp(_service, _trelloMemberID);
            return View(dashboardViewModel);
        }

        public async Task<ActionResult> Board(string boardID)
        {
            if (_userInformationMissing)
                return RedirectToAction("Login", "Account");

            var board = await _service.GetBoard(boardID);
            var cards = await _service.GetCardsForBoard(boardID);
            var lists = await _service.GetListsForBoard(boardID);

            foreach (var list in lists)
            {
                list.Cards = cards.Where(c => c.IDList == list.ID);
            }

            board.Lists = lists;

            return View(board);
        }

        [HttpGet]
        public async Task<ActionResult> Card(string cardID)
        {
            if (_userInformationMissing)
                return RedirectToAction("Login", "Account");

            var card = await _service.GetCard(cardID);
            var comments = await _service.GetCommentsForCard(cardID);

            card.Comments = comments;

            return View(card);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(string cardID, string comment)
        {
            if(_userInformationMissing)
                return RedirectToAction("Login", "Account");

            await _service.AddComment(cardID, comment);

            return RedirectToAction("Card", "Dashboard", new { cardID = cardID });
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

            var isUserCookieSet = userCookie != null || string.IsNullOrWhiteSpace(userCookie.Value) == false;

            User user = null;

            if (isUserCookieSet)
            {
                var userID = userCookie.Value;
                user = _db.Users.FirstOrDefault(u => u.id == userID);
            }

            if (isUserCookieSet == false || user == null)
            {
                _userInformationMissing = true;
                _service = null;
                base.OnActionExecuting(filterContext);
                return;
            }

            _trelloMemberID = user.id;
            _service = new TrelloService(user.TrelloToken);

            base.OnActionExecuting(filterContext);
        }
    }
}