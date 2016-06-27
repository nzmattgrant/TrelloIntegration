using System;
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
            var cardID = "test card ID";

            var card = TestHelpers.CreateTestCardWithCardID(cardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetCard(cardID, trelloTestToken)).Returns(Task.FromResult(card));
            var cardDetailViewModel = new CardDetailViewModel();
            await cardDetailViewModel.SetUp(mockService.Object, user, cardID);

            cardDetailViewModel.Card.Should().BeSameAs(card);
        }


        [TestMethod]
        public async Task SetUp_Should_AddCardWithComments()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            var cardID = "test card ID";
            var commentID = "test comment ID";

            var card = TestHelpers.CreateTestCardWithCardID(cardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            var comment = TestHelpers.CreateTestCommentWithCommentID(commentID);
            var comments = TestHelpers.CreateTestCommentListFromComment(comment);

            mockService.Setup(s => s.GetCard(cardID, trelloTestToken)).Returns(Task.FromResult(card));
            mockService.Setup(s => s.GetCommentsForCard(cardID, trelloTestToken)).Returns(Task.FromResult((IEnumerable<Comment>)comments));
            var cardDetailViewModel = new CardDetailViewModel();
            await cardDetailViewModel.SetUp(mockService.Object, user, cardID);

            cardDetailViewModel.Card.Comments.Should().BeEquivalentTo(comments);
        }
    }
}
