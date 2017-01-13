namespace Alpha.Reports.Site.Tests.Handlers
{
    using NSubstitute;
    using Reports.Models;
    using Services.Outstanding;
    using Site.Handlers.Outstanding;
    using Site.Requests.Outstanding;
    using Xunit;

    [Trait("Category", "Outstanding")]
    public class OutstandingHandlerTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(2)]
        public void Handle_WhenSmaCalls_DereaseReportLevelByOne(byte reportLevelId)
        {
            // Arrange
            var service = Substitute.For<OutstandingService>();
            var user = Substitute.For<IUser>();
            var handler = new OutstandingHandler(service, user);
            var query = new OutstandingQuery(user);
            query.CustLevelId = reportLevelId;

            // Act
            var viewModel = handler.Handle(query);

            // Assert
            Assert.Equal(viewModel.ReportLevel.Value, reportLevelId - 1);
        }
    }
}