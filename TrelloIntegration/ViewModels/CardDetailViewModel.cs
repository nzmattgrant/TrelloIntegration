using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class CardDetailViewModel
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