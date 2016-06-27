using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.Services;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Tests.ViewModels
{
    [TestClass]
    public class BoardDetailViewModelTests
    {
        [TestMethod]
        public async Task SetUp_Should_AddBoard()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            var boardID = "test board ID";

            //Set up unnested cards and test they are nested after setup
            var board = TestHelpers.CreateTestBoardWithBoardID(boardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(board));
            var boardViewModel = new BoardDetailViewModel();
            await boardViewModel.SetUp(mockService.Object, user, boardID);

            boardViewModel.Board.Should().BeSameAs(board);
        }

        [TestMethod]
        public async Task SetUp_Should_AddBoardWithLists()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            var boardID = "test board ID";
            var listID = "test list ID";

            //Set up unnested cards and test they are nested after setup
            var list = TestHelpers.CreateTestListWithListID(listID);
            list.IDBoard = boardID;
            var lists = TestHelpers.CreateTestListsListFromList(list);
            var board = TestHelpers.CreateTestBoardWithBoardID(boardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(board));
            mockService.Setup(s => s.GetListsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<List>)lists));
            var boardViewModel = new BoardDetailViewModel();
            await boardViewModel.SetUp(mockService.Object, user, boardID);

            boardViewModel.Board.Lists.Should().BeEquivalentTo(lists);
        }

        [TestMethod]
        public async Task SetUp_Should_AddBoardWithCards()
        {
            var mockService = new Mock<ITrelloService>();

            var trelloTestToken = "TestTrelloToken";
            var boardID = "test board ID";
            var listID = "test list ID";
            var cardID = "test card ID";

            //Set up unnested cards and test they are nested after setup
            var card = TestHelpers.CreateTestCardWithCardID(cardID);
            card.IDList = listID;
            card.IDBoard = boardID;
            var cards = TestHelpers.CreateTestCardsListFromCard(card);
            var list = TestHelpers.CreateTestListWithListID(listID);
            list.IDBoard = boardID;
            var lists = TestHelpers.CreateTestListsListFromList(list);
            var board = TestHelpers.CreateTestBoardWithBoardID(boardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(board));
            mockService.Setup(s => s.GetListsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<List>)lists));
            mockService.Setup(s => s.GetCardsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<Card>)cards));
            var boardViewModel = new BoardDetailViewModel();
            await boardViewModel.SetUp(mockService.Object, user, boardID);

            boardViewModel.Board.Lists.First().Cards.Should().BeEquivalentTo(cards);
        }

    }
}
