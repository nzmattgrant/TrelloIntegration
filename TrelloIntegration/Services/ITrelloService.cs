using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Services
{
    public interface ITrelloService
    {
        Task<string> GetMemberIDForUserToken(string userToken);

        Task<User> GetMemberForUserToken(string userToken);

        Task<BoardViewModel> GetBoard(string boardID, string userToken);

        Task<CardViewModel> GetCard(string cardID, string userToken);

        Task<IEnumerable<BoardViewModel>> GetBoardsForUser(string memberID, string userToken);

        Task<IEnumerable<ListViewModel>> GetListsForBoard(string boardID, string userToken);

        Task<IEnumerable<CardViewModel>> GetCardsForBoard(string boardID, string userToken);

        Task<IEnumerable<CommentViewModel>> GetCommentsForCard(string cardID, string userToken);

        Task AddComment(string cardID, string comment, string userToken);
    }
}