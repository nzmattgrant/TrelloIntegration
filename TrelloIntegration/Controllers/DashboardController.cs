using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TrelloIntegration.DAL;
using TrelloIntegration.Models;
using TrelloIntegration.Services;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Controllers
{
    public class DashboardController : Controller
    {
        private const string TRELLO_USER_COOKIE_NAME = "TrelloIntegrationUser";
        private TrelloIntegrationContext _db { get; set; }
        private ITrelloService _service { get; set; }

        public DashboardController()
        {
            _service = new TrelloService();
            _db = new TrelloIntegrationContext();
        }

        //TODO Set this up with dependancy injection 
        public DashboardController(ITrelloService service, TrelloIntegrationContext db)
        {
            _service = service;
            _db = db;
        }


        public async Task<ActionResult> Index()
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.Boards = await _service.GetBoardsForUser(user.ID, user.TrelloToken);
            return View(dashboardViewModel);
        }

        public async Task<ActionResult> Board(string boardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var board = await _service.GetBoard(boardID, user.TrelloToken);
            var cards = await _service.GetCardsForBoard(boardID, user.TrelloToken);
            var lists = await _service.GetListsForBoard(boardID, user.TrelloToken);

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
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var card = await _service.GetCard(cardID, user.TrelloToken);
            var comments = await _service.GetCommentsForCard(cardID, user.TrelloToken);

            card.Comments = comments;

            return View(card);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(string cardID, string comment)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            await _service.AddComment(cardID, comment, user.TrelloToken);

            return RedirectToAction("Card", "Dashboard", new { cardID = cardID });
        }

        private User getUserUsingCookie()
        {
            User user = null;

            var userCookie = HttpContext.Request.Cookies.Get(TRELLO_USER_COOKIE_NAME);

            var isUserCookieSet = userCookie != null && string.IsNullOrWhiteSpace(userCookie.Value) == false;

            if (isUserCookieSet)
            {
                var userID = userCookie.Value;
                user = _db.Users.FirstOrDefault(u => u.ID == userID);
            }

            return user;
        }
    }
}