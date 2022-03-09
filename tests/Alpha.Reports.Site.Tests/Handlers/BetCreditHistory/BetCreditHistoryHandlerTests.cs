namespace Alpha.Reports.Site.Tests.Handlers.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Tests.Extensions;
    using Alpha.Reports.Site.Tests.Fixtures;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    [Collection("UserCollection")]
    public class BetCreditHistoryHandlerTests
    {
        private readonly BetCreditHistoryService reportService;
        private readonly BetCreditHistoryHandler handler;
        private readonly IUser user;

        public BetCreditHistoryHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<BetCreditHistoryService>();
            handler = new BetCreditHistoryHandler(reportService);
            user = userFixture.User;
        }

        [Fact]
        public void Handle_Always_CallsServiceToGetBetCreditHistory()
        {
            // Arrange
            var message = new BetCreditHistoryQuery(user);

            var resultModel = new BetCreditHistoryResultModel(0, "Success", null);
            reportService.GetBetCreditHistory(Arg.Any<BetCreditHistoryParam>()).Returns(resultModel);

            // Act
            handler.Handle(message);

            // Assert
            reportService.Received().GetBetCreditHistory(Arg.Is<BetCreditHistoryParam>(param
                => param != null
                && param.Member == message.Member
                && param.RefNo == message.RefNo
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
            BetCreditHistoryQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => handler.Handle(nullMessage));
        }

        [Fact]
        public void Handle_Valid_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new BetCreditHistoryQuery(user);

            var rawData = new List<BetCreditHistoryModel>
            {
                new BetCreditHistoryModel(DateTime.Now.AddDays(-1), DateTime.Now, 123456, 10, 10, 10, 10, 10, 10, 10, "Bet Updated"),
                new BetCreditHistoryModel(DateTime.Now.AddDays(-2), DateTime.Now, 234567, 20, 20, 20, 20, 20, 20, 20, "Bet Updated")
            };
            var resultModel = new BetCreditHistoryResultModel(0, "Success", rawData);
            reportService.GetBetCreditHistory(Arg.Any<BetCreditHistoryParam>()).Returns(resultModel);

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
            var message = new BetCreditHistoryQuery(user);

            var resultModel = new BetCreditHistoryResultModel(101, "Invalid member", null);
            reportService.GetBetCreditHistory(Arg.Any<BetCreditHistoryParam>()).Returns(resultModel);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.True(actualViewModel.Data == null && actualViewModel.ErrMsg != string.Empty);
        }
    }
}
