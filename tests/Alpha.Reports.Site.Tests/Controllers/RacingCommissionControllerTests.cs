using Alpha.Reports.Models;
using Alpha.Reports.Site.Controllers;
using Alpha.Reports.Site.Requests.RacingCommission;
using Alpha.Reports.Site.Tests.Fixtures;
using Alpha.Reports.Site.ViewModels;
using MediatR;
using NSubstitute;
using Xunit;

namespace Alpha.Reports.Site.Tests.Controllers
{
    [Trait("Category", "RacingCommission")]
    [Collection("UserCollection")]
    public class RacingCommissionControllerTests
    {
        private readonly IMediator mediator;
        private readonly RacingCommissionController controller;
        private readonly IUser user;

        public RacingCommissionControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new RacingCommissionController(mediator);
            user = userFixture.User;
        }

        [Fact]
        public void Index_Always_CallsMediatorSendRacingCommissionQuery()
        {
            // Arrange
            var message = new RacingCommissionQuery(user);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendRacingCommissionExcelQuery()
        {
            // Arrange
            var message = new RacingCommissionExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<RacingCommissionExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
