﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Services
{
    public interface ITrelloService
    {
        Task<string> GetMemberIDForUserToken(string userToken);

        Task<User> GetMemberForUserToken(string userToken);

        Task<Board> GetBoard(string boardID, string userToken);

        Task<Card> GetCard(string cardID, string userToken);

        Task<IEnumerable<Board>> GetBoardsForUser(string memberID, string userToken);

        Task<IEnumerable<List>> GetListsForBoard(string boardID, string userToken);

        Task<IEnumerable<Card>> GetCardsForBoard(string boardID, string userToken);

        Task<IEnumerable<Comment>> GetCommentsForCard(string cardID, string userToken);

        Task AddComment(string cardID, string comment, string userToken);
    }
}