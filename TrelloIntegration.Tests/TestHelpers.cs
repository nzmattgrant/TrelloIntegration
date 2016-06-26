using System.Collections.Generic;
using TrelloIntegration.Models;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Tests
{
    public static class TestHelpers
    {
        public static User CreateTestUser()
        {
            return new User
            {
                ID = "test ID",
                FullName = "test full name",
                TrelloToken = "test trello token"
            };
        }

        public static User CreateTestUserWithTrelloToken(string token)
        {
            return new User
            {
                ID = "test ID",
                FullName = "test full name",
                TrelloToken = token
            };
        }

        public static Comment CreateTestCommentWithCommentID(string commentID)
        {
            return new Comment()
            {
                ID = commentID,
                Data = new CommentDataViewModel()
                {
                    Text = "Test comment text"
                }
            };
        }

        public static List<Comment> CreateTestCommentListFromComment(Comment comment)
        {
            return new List<Comment>()
            {
                comment
            };
        }

        public static List<Card> CreateTestCardsListFromCard(Card card)
        {
            return new List<Card>()
            {
                card
            };
        }

        public static Card CreateTestCardWithCardID(string cardID)
        {
            return new Card()
            {
                ID = cardID,
                Name = "Test card name",
            };
        }

        public static List<List> CreateTestListsListFromList(List list)
        {
            return new List<List>()
            {
                list
            };
        }

        public static List CreateTestListWithListID(string listID)
        {
            return new List()
            {
                ID = listID,
                Name = "Test card name",
            };
        }

        public static Board CreateTestBoardWithBoardID(string boardID)
        {
            return new Board
            {
                ID = boardID,
                Name = "Test board name"
            };
        }

        public static BoardViewModel CreateTestBoardViewModelWithBoard(Board board)
        {
            return new BoardViewModel
            {
                Board = board
            };
        }
    }
}
