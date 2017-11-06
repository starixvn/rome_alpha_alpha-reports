namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Requests.CommissionByBettype;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "CommissionByBettype")]
    [Collection("UserCollection")]
    public class CommissionByBettypeControllerTests
    {
        private readonly IMediator mediator;
        private readonly CommissionByBettypeController controller;
        private readonly IUser user;

        public CommissionByBettypeControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new CommissionByBettypeController(mediator);
            user = userFixture.User;
        }

        [Fact]
        public void Index_Always_CallsMediatorSendCommissionByBettypeQuery()
        {
            // Arrange
            var message = new CommissionByBettypeQuery(user);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendCommissionByBettypeExcelQuery()
        {
            // Arrange
            var message = new CommissionByBettypeExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<CommissionByBettypeExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }

        [Fact]
        public void Guidelines_Always_CallMediatorSendCommissionByBetTypeGuideQuery()
        {
            //// Arrange
            var message = new CommissionByBetTypeGuideQuery(user);

            //// Act
            controller.Guidelines(message);

            //// Assert
            mediator.Received().Send(message);
        }
    }
}
