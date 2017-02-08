namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossByBetType;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossByBetTypeController controller;
        private readonly WinLossByBetTypeQuery message;

        public WinLossByBetTypeControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new WinLossByBetTypeController(mediator);
            message = new WinLossByBetTypeQuery(userFixture.User);
        }

        [Fact]
        public void WinLossByBettype_Always_ReturnIndex()
        {
            // Arrange

            // Act
            ViewResult result = controller.Index(message) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void WinLossByBettype_Always_CallsMediator()
        {
            // Arrange

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(Arg.Any<WinLossByBetTypeQuery>());
        }

        [Fact]
        public void WinLossByBettype_CallsMediator_MustSendWinLossByBettypeQuery()
        {
            // Arrange

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }
    }
}