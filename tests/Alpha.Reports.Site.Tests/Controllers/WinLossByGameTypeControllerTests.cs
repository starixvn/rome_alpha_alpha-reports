namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossByGameType;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    [Collection("UserCollection")]
    public class WinLossByGameTypeControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossByGameTypeController controller;
        private readonly WinLossByGameTypeQuery message;

        public WinLossByGameTypeControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new WinLossByGameTypeController(mediator);
            message = new WinLossByGameTypeQuery(userFixture.User);
        }

        [Fact]
        public void WinLossByGameType_Always_ReturnIndex()
        {
            // Arrange

            // Act
            ViewResult result = controller.Index(message) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void WinLossByGameType_Always_CallsMediator()
        {
            // Arrange

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(Arg.Any<WinLossByGameTypeQuery>());
        }

        [Fact]
        public void WinLossByGameType_CallsMediator_MustSendWinLossByGameTypeQuery()
        {
            // Arrange

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }
    }
}