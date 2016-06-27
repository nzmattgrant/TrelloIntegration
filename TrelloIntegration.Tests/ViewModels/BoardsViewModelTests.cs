using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.Services;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Tests.ViewModels
{
    [TestClass]
    public class BoardsViewModelTests
    {
        [TestMethod]
        public async Task SetUp_Should_AddBoards()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            var boardID = "test board ID";

            var board = TestHelpers.CreateTestBoardWithBoardID(boardID);
            var boards = TestHelpers.CreateTestBoardsListFromBoard(board);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetBoardsForUser(user.ID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<Board>)boards));
            var boardsViewModel = new BoardsViewModel();
            await boardsViewModel.SetUp(mockService.Object, user);

            boardsViewModel.Boards.Should().BeEquivalentTo(boards);
        }
    }
}
