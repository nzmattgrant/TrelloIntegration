using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TrelloIntegration.Services;
using Moq;
using TrelloIntegration.ViewModels;
using FluentAssertions;

namespace TrelloIntegration.Tests.ViewModels
{
    [TestClass]
    public class CardViewModelTests
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
            var cardViewModel = new CardViewModel();
            await cardViewModel.SetUp(mockService.Object, user, cardID);

            cardViewModel.Card.Should().BeSameAs(card);
        }

        [TestMethod]
        public async Task SetUp_Should_AddUserName()
        {
            var mockService = new Mock<ITrelloService>();
            var trelloTestToken = "TestTrelloToken";
            var cardID = "test card ID";

            var card = TestHelpers.CreateTestCardWithCardID(cardID);
            var user = TestHelpers.CreateTestUserWithTrelloToken(trelloTestToken);

            mockService.Setup(s => s.GetCard(cardID, trelloTestToken)).Returns(Task.FromResult(card));
            var cardViewModel = new CardViewModel();
            await cardViewModel.SetUp(mockService.Object, user, cardID);

            cardViewModel.UserFullName.Should().BeSameAs(user.FullName);
        }
    }
}
