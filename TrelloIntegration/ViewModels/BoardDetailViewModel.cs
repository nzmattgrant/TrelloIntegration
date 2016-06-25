using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TrelloIntegration.Models;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class BoardDetailViewModel
    {
        public BoardViewModel Board { get; set; }

        public async Task SetUp(ITrelloService service, User user, string boardID)
        {
            Board = await service.GetBoard(boardID, user.TrelloToken);
            var cards = await service.GetCardsForBoard(boardID, user.TrelloToken);
            var lists = await service.GetListsForBoard(boardID, user.TrelloToken);

            foreach (var list in lists)
            {
                list.Cards = cards.Where(c => c.IDList == list.ID);
            }

            Board.Lists = lists;
        }
    }
}