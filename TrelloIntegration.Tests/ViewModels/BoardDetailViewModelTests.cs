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
            var boardViewModel = new BoardViewModel
            {
                ID = boardID,
                Name = testBoardName
            }; 

            var boardDetailViewModel = new BoardDetailViewModel()
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
            var dashboardViewModel = new BoardDetailViewModel();
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

            var listViewModels = new List<ListViewModel>()
            {
                new ListViewModel ()
                {
                    ID  = "TestListID1"
                },
                new ListViewModel()
                {
                    ID  = "TestListID2"
                }
            };

            var boardViewModel = new BoardViewModel
            {
                ID = boardID,
                Name = testBoardName
            };

            var boardDetailViewModel = new BoardDetailViewModel()
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
            mockService.Setup(s => s.GetListsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<ListViewModel>)listViewModels));
            var dashboardViewModel = new BoardDetailViewModel();
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
            var cardViewModels = new List<CardViewModel>()
            {
                new CardViewModel ()
                {
                    ID  = "TestCardID1",
                    Name = "Test card name",
                    IDList = firstListID
                }
            };

            var listViewModels = new List<ListViewModel>()
            {
                new ListViewModel ()
                {
                    ID  = firstListID,
                    Name = "Test card name",
                    Cards = cardViewModels
                }
            };

            var boardViewModel = new BoardViewModel
            {
                ID = boardID,
                Name = testBoardName
            };

            var boardDetailViewModel = new BoardDetailViewModel()
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
            mockService.Setup(s => s.GetListsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<ListViewModel>)listViewModels));
            mockService.Setup(s => s.GetCardsForBoard(boardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<CardViewModel>)cardViewModels));
            var dashboardViewModel = new BoardDetailViewModel();
            await dashboardViewModel.SetUp(mockService.Object, user, boardID);

            dashboardViewModel.Board.Lists.First().Cards.First().Should().BeSameAs(cardViewModels.First());
        }

    }
}
