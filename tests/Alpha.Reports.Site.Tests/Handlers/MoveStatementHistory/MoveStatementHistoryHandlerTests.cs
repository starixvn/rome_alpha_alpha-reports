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
        public void Handle_Always_CallsServiceToGetMoveStatementHistory()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);

            var resultModel = new MoveStatementHistoryResultModel(0, "Success", null);
            reportService.GetMoveStatementHistory(Arg.Any<MoveStatementHistoryParam>()).Returns(resultModel);

            // Act
            handler.Handle(message);

            // Assert
            reportService.Received().GetMoveStatementHistory(Arg.Is<MoveStatementHistoryParam>(param
                => param != null
                && param.MemberUsername == message.MemberUsername
                && param.FromDate == message.FromDate
                && param.ToDate == message.ToDate
                && param.ViewerId == message.ViewerId
                && param.LimitedDateToShowInfo == message.LimitedDateToShowInfo
                ));
        }

        [Fact]
        public void Handle_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            MoveStatementHistoryQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => handler.Handle(nullMessage));
        }

        [Fact]
        public void Handle_Valid_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);

            var refNoRow = new List<MoveStatementHistoryByRefNoModel>
            {
                new MoveStatementHistoryByRefNoModel("member1", 1, DateTime.Now.AddDays(-1), DateTime.Now, 10, 10, 10, 1),
                new MoveStatementHistoryByRefNoModel("member1", 1, DateTime.Now.AddDays(-2), DateTime.Now, 10, 10, 10, 1)
            };

            var rawData = new List<MoveStatementHistoryByMemberModel>
            {
                new MoveStatementHistoryByMemberModel("member1", refNoRow)
            };
            var resultModel = new MoveStatementHistoryResultModel(0, "Success", rawData);
            reportService.GetMoveStatementHistory(Arg.Any<MoveStatementHistoryParam>()).Returns(resultModel);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(actualViewModel.Data, rawData);
            Assert.True(actualViewModel.ErrMsg == string.Empty);
        }

        [Fact]
        public void Handle_Invalid_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);

            var resultModel = new MoveStatementHistoryResultModel(101, "Invalid member", null);
            reportService.GetMoveStatementHistory(Arg.Any<MoveStatementHistoryParam>()).Returns(resultModel);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.True(actualViewModel.Data == null && actualViewModel.ErrMsg != string.Empty);
        }
    }
}
