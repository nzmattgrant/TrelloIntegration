
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class CardViewModel : DashboardViewModel
    {
        public Card Card { get; set; }

        public async Task SetUp(ITrelloService service, User user, string cardID)
        {
            Card = await service.GetCard(cardID, user.TrelloToken);
            UserFullName = user.FullName;
        }
    }
}