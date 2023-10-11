namespace Alpha.Reports.Site.Tests.Handlers.MoveStatementHistory
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Tests.Extensions;
    using Alpha.Reports.Site.Tests.Fixtures;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    [Collection("UserCollection")]
    public class MoveStatementHistoryHandlerTests
    {
        private readonly MoveStatementHistoryService reportService;
        private readonly MoveStatementHistoryHandler handler;
        private readonly IUser user;

        public MoveStatementHistoryHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<MoveStatementHistoryService>();
            handler = new MoveStatementHistoryHandler(reportService);
            user = userFixture.User;
        }

        [Fact]
        public async Task Handle_Always_CallsServiceToGetMoveStatementHistory()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);
            var summary = new MoveStatementHistorySummary(0, 0, 0, 0, 0);

            var resultModel = new MoveStatementHistoryResult(0, "Success", null, summary, 0);
            reportService.GetMoveStatementHistory(Arg.Any<MoveStatementHistoryParam>()).Returns(resultModel);

            // Act
            await handler.Handle(message);

            // Assert
            await reportService.Received().GetMoveStatementHistory(Arg.Is<MoveStatementHistoryParam>(param
                => param != null
                && param.Username == message.Username
                && param.FromDate == message.FromDate
                && param.ToDate == message.ToDate
                && param.ViewerId == message.ViewerId
                && param.LimitedDateToShowInfo == message.LimitedDateToShowInfo
                ));
        }

        [Fact]
        public async Task Handle_Valid_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);

            var refNoRow = new List<MoveStatementHistoryByRefNo>
            {
                new MoveStatementHistoryByRefNo(1, "member1", 2, DateTime.Now.AddDays(-1), DateTime.Now, 10, 10, 10, 1),
                new MoveStatementHistoryByRefNo(1, "member1", 2, DateTime.Now.AddDays(-2), DateTime.Now, 10, 10, 10, 1)
            };

            var memberMoveStatementHistoryByRefNo = new MoveStatementHistoryByRefNo
                (1, "member1", 2, DateTime.Now.AddDays(-1), DateTime.Now, 20, 20, 20, 1);

            var rawData = new List<MoveStatementHistoryByMember>
            {
                new MoveStatementHistoryByMember("member1", memberMoveStatementHistoryByRefNo, refNoRow)
            };
            var summary = new MoveStatementHistorySummary(1, 20, 20, 20, 0);
            var resultModel = new MoveStatementHistoryResult(0, "Success", rawData, summary, 2);
            reportService.GetMoveStatementHistory(Arg.Any<MoveStatementHistoryParam>()).Returns(resultModel);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            AssertExtension.Equal(actualViewModel.Data, rawData);
            Assert.True(actualViewModel.ErrMsg == string.Empty);
        }

        [Fact]
        public async Task Handle_Invalid_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);
            var summary = new MoveStatementHistorySummary(0, 0, 0, 0, 0);

            var resultModel = new MoveStatementHistoryResult(101, "Invalid member", null, summary, 0);
            reportService.GetMoveStatementHistory(Arg.Any<MoveStatementHistoryParam>()).Returns(resultModel);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            Assert.True(actualViewModel.Data == null && actualViewModel.ErrMsg != string.Empty);
        }
    }
}
