using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrelloIntegration.Services;

namespace TrelloIntegration.Controllers
{
    public class DashboardController : Controller
    {

        private TrelloService service { get; set; }

        public DashboardController()
        {
            //TODO Got to figure out how to get the user token here (this will come with the persistance of the user)
            service = new TrelloService("d7a6788e5cbaad0168bfd9487a880ea18031f22b7dde64b166f81dffeb9f2d80");
        }

        public async Task<ActionResult> Boards()
        {
            var memberID = await service.GetMemberIDForUserToken();
            var boards = await service.GetBoardsForUser(memberID);
            return View(boards);
        }

        public async Task<ActionResult> Lists(string boardID)
        {
            return View(await service.GetListsForBoard(boardID));
        }

        public async Task<ActionResult> Cards(string listID)
        {
            return View(await service.GetCardsForList(listID));
        }

        public async Task<ActionResult> Comments(string cardID)
        {
            return View(await service.GetCommentsForCard(cardID));
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}