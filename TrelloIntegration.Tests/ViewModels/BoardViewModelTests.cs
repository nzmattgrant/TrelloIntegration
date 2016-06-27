using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrelloIntegration.ViewModels;
using TrelloIntegration.Services;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;

namespace TrelloIntegration.Tests.ViewModels
{
    [TestClass]
    public class BoardViewModelTests
    {
        [TestMethod]
        public async Task SetUp_Should_AddBoard()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            var boardID = "test board ID";

            var board = TestHelpers.CreateTestBoardWithBoardID(boardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(board));
            var boardViewModel = new BoardViewModel();
            await boardViewModel.SetUp(mockService.Object, user, boardID);

            boardViewModel.Board.Should().BeSameAs(board);
        }

        [TestMethod]
        public async Task SetUp_Should_AddUserName()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            var boardID = "test board ID";

            var board = TestHelpers.CreateTestBoardWithBoardID(boardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(board));
            var boardViewModel = new BoardViewModel();
            await boardViewModel.SetUp(mockService.Object, user, boardID);

            boardViewModel.UserFullName.Should().BeSameAs(user.FullName);
        }
    }
}
