using System.Linq;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class BoardViewModel : DashboardViewModel
    { 
        public Board Board { get; set; }

        public static async Task<BoardViewModel> Create(ITrelloService service, User user, string boardID)
        {
            return new BoardViewModel
            {
                Board = await service.GetBoard(boardID, user.TrelloToken),
                UserFullName = user.FullName
            };
        }
    }
}