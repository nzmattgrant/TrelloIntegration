using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.Services;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Tests.ViewModels
{
    [TestClass]
    public class BoardDetailViewModelTests
    {
        public BoardDetailViewModelTests()
        {
            //TODO set up the boards            
        }

        [TestMethod]
        public async Task SetUp_Should_AddBoard()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            string boardID = "test board ID";
            string testBoardName = "test board ID";
            var boardViewModel = new Board
            {
                ID = boardID,
                Name = testBoardName
            }; 

            var boardDetailViewModel = new BoardViewModel()
            {
                Board = boardViewModel
            };

            var user = new User
            {
                ID = "TestID",
                FullName = "TestFullName",
                TrelloToken = trelloTestToken
            };

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(boardViewModel));
            var dashboardViewModel = new BoardViewModel();
            await dashboardViewModel.SetUp(mockService.Object, user, boardID);

            dashboardViewModel.Board.Should().BeSameAs(boardViewModel);
        }

        [TestMethod]
        public async Task SetUp_Should_AddBoardWithLists()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            string boardID = "test board ID";
            string testBoardName = "test board ID";

            var listViewModels = new List<List>()
            {
                new List ()
                {
                    ID  = "TestListID1"
                },
                new List()
                {
                    ID  = "TestListID2"
                }
            };

            var boardViewModel = new Board
            {
                ID = boardID,
                Name = testBoardName
            };

            var boardDetailViewModel = new BoardViewModel()
            {
                Board = boardViewModel
            };

            var user = new User
            {
                ID = "TestID",
                FullName = "TestFullName",
                TrelloToken = trelloTestToken
            };

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(boardViewModel));
            mockService.Setup(s => s.GetListsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<List>)listViewModels));
            var dashboardViewModel = new BoardViewModel();
            await dashboardViewModel.SetUp(mockService.Object, user, boardID);

            dashboardViewModel.Board.Lists.Should().BeSameAs(listViewModels);
        }

        [TestMethod]
        public async Task SetUp_Should_AddBoardWithCards()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            string boardID = "test board ID";
            string testBoardName = "test board ID";
            string firstListID = "TestListID1";
            var cardViewModels = new List<Card>()
            {
                new Card ()
                {
                    ID  = "TestCardID1",
                    Name = "Test card name",
                    IDList = firstListID
                }
            };

            var listViewModels = new List<List>()
            {
                new List ()
                {
                    ID  = firstListID,
                    Name = "Test card name",
                    Cards = cardViewModels
                }
            };

            var boardViewModel = new Board
            {
                ID = boardID,
                Name = testBoardName
            };

            var boardDetailViewModel = new BoardViewModel()
            {
                Board = boardViewModel
            };

            var user = new User
            {
                ID = "TestID",
                FullName = "TestFullName",
                TrelloToken = trelloTestToken
            };

            mockService.Setup(s => s.GetBoard(boardID, trelloTestToken)).Returns(Task.FromResult(boardViewModel));
            mockService.Setup(s => s.GetListsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<List>)listViewModels));
            mockService.Setup(s => s.GetCardsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<Card>)cardViewModels));
            var dashboardViewModel = new BoardViewModel();
            await dashboardViewModel.SetUp(mockService.Object, user, boardID);

            dashboardViewModel.Board.Lists.First().Cards.First().Should().BeSameAs(cardViewModels.First());
        }

    }
}
