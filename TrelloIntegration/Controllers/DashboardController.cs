using System.Linq;
using System.Threading.Tasks;
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

        //TODO Set this up with dependancy injection 
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
                return RedirectToAction("Login", "Account");

            return PartialView(await BoardsViewModel.Create(_service, user));
        }

        [HttpGet]
        public async Task<ActionResult> Board(string boardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

            var boardViewModel = await BoardViewModel.Create(_service, user, boardID);

            return View(boardViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> BoardDetail(string boardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");
                //TODO return json result?

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

            var cardViewModel = await CardViewModel.Create(_service, user, cardID);

            return View(cardViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> CardDetail(string cardID)
        {
            var user = getUserUsingCookie();
            if (user == null)
                return RedirectToAction("Login", "Account");

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