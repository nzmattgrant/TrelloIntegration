﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TrelloIntegration.Services;
using Moq;
using TrelloIntegration.ViewModels;
using TrelloIntegration.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace TrelloIntegration.Tests.ViewModels
{
    [TestClass]
    public class CardDetailViewModelTests
    {
        [TestMethod]
        public async Task SetUp_Should_AddCard()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            string cardID = "test card ID";
            string testCardName = "test card ID";
            var cardViewModel = new Card()
            {
                ID = cardID,
                Name = testCardName
            };

            var CardDetailViewModel = new CardViewModel()
            {
                Card = cardViewModel
            };

            var user = new User
            {
                ID = "TestID",
                FullName = "TestFullName",
                TrelloToken = trelloTestToken
            };

            mockService.Setup(s => s.GetCard(cardID, trelloTestToken)).Returns(Task.FromResult(cardViewModel));
            var cardDetailViewModel = new CardViewModel();
            await cardDetailViewModel.SetUp(mockService.Object, user, cardID);

            cardDetailViewModel.Card.Should().BeSameAs(cardViewModel);
        }


        [TestMethod]
        public async Task SetUp_Should_AddCardWithComments()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            string cardID = "test card ID";
            string testCardName = "test card ID";

            var commentViewModels = new List<Comment>()
            {
                new Comment()
                {
                    ID = "test comment ID",
                    Data = new CommentDataViewModel()
                    {
                        Text = "Test comment text"
                    } 
                }
            };

            var cardViewModel = new Card()
            {
                ID = cardID,
                Name = testCardName
            };

            var boardDetailViewModel = new CardViewModel()
            {
                Card = cardViewModel
            };

            var user = new User
            {
                ID = "TestID",
                FullName = "TestFullName",
                TrelloToken = trelloTestToken
            };

            mockService.Setup(s => s.GetCard(cardID, trelloTestToken)).Returns(Task.FromResult(cardViewModel));
            mockService.Setup(s => s.GetCommentsForCard(cardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<Comment>)commentViewModels));
            var cardDetailViewModel = new CardViewModel();
            await cardDetailViewModel.SetUp(mockService.Object, user, cardID);

            cardDetailViewModel.Card.Should().BeSameAs(cardViewModel);
        }
    }
}
