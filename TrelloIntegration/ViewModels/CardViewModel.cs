
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class CardViewModel : DashboardViewModel
    {
        public Card Card { get; set; }

        public static async Task<CardViewModel> Create(ITrelloService service, User user, string cardID)
        {
            return new CardViewModel
            {
                Card = await service.GetCard(cardID, user.TrelloToken),
                UserFullName = user.FullName
            };
        }
    }
}