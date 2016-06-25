using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TrelloIntegration.Controllers;
using TrelloIntegration.Services;

namespace TrelloIntegration.Tests.Controllers.Dashboard
{
    [TestClass]
    public class DashboardControllerTests
    {
        private DashboardController _controller;

        public DashboardControllerTests()
        {
            var mockService = new Mock<ITrelloService>();
            _controller = new DashboardController();
        }

        [TestMethod]
        public async Task TestDashboardController()
        {
            var result = await _controller.Board("1");
            result.Should().BeOfType<string>();
        }
        [TestMethod]
        public void MyTestMethod()
        {

        }
    }
}
