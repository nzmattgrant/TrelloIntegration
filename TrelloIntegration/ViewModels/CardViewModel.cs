using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TrelloIntegration.Models;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class CardViewModel
    {
        public Card Card { get; set; }

        public async Task SetUp(ITrelloService service, User user, string cardID)
        {
            Card = await service.GetCard(cardID, user.TrelloToken);
            var comments = await service.GetCommentsForCard(cardID, user.TrelloToken);
            Card.Comments = comments;
        }
    }
}