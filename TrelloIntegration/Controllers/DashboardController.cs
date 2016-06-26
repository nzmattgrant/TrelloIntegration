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
        private ITrelloIntegrationContext _db { get; set; }
        private ITrelloService _service { get; set; }

        public DashboardController()
        {
            _service = new TrelloService();
            _db = new TrelloIntegrationContext();
        }

        public DashboardController(ITrelloService service, ITrelloIntegrationContext db)
        {
            _service = service;
            _db = db;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");
            
            return View(new DashboardViewModel(user.FullName));
        }

        [HttpGet]
        public async Task<ActionResult> Boards()
        {
            var user = getUserUsingCookie();
            if (user == null)
                //Return a http error instead of redirecting because this is a partial being retreived via Ajax
                throw new HttpException(401, "Not authorized");
            var boardsViewModel = new BoardsViewModel();
            await boardsViewModel.SetUp(_service, user);

            return PartialView(boardsViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Board(string boardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var boardViewModel = new BoardViewModel();
            await boardViewModel.SetUp(_service, user, boardID);

            if (string.IsNullOrWhiteSpace(boardViewModel.Board.ID))
                throw new HttpException(404, "Not found");

            return View(boardViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> BoardDetail(string boardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                //Return a http error instead of redirecting because this is a partial being retreived via Ajax
                throw new HttpException(401, "Not authorized");

            var boardDetailViewModel = new BoardDetailViewModel();
            await boardDetailViewModel.SetUp(_service, user, boardID);

            return PartialView(boardDetailViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Card(string cardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cardViewModel = new CardViewModel();
            await cardViewModel.SetUp(_service, user, cardID);

            if (string.IsNullOrWhiteSpace(cardViewModel.Card.ID))
                throw new HttpException(404, "Not found");

            return View(cardViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> CardDetail(string cardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                //Return a http error instead of redirecting because this is a partial being retreived via Ajax
                throw new HttpException(401, "Not authorized");

            var cardDetailViewModel = new CardDetailViewModel();
            await cardDetailViewModel.SetUp(_service, user, cardID);

            return PartialView(cardDetailViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(string cardID, string comment)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var response = await _service.AddComment(cardID, comment, user.TrelloToken);

            TempData["wasCommentAdded"] = response.IsSuccessStatusCode;

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