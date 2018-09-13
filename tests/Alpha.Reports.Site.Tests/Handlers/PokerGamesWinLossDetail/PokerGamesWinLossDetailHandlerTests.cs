namespace Alpha.Reports.Site.Tests.Handlers.PokerGamesWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.PokerGamesWinLossDetail;
    using Services;
    using Site.Handlers.PokerGamesWinLossDetail;
    using Site.Requests.PokerGamesWinLossDetail;
    using Site.ViewModels.PokerGamesWinLossDetail;
    using Utils;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    [Collection("UserCollection")]
    public class PokerGamesWinLossDetailHandlerTests
    {
        private readonly PokerGamesWinLossDetailHandler winLossDetailHandler;
        private readonly UserFixture userFixture;
        private readonly PagedReportData<PokerGamesWinLossDetailModel> reportData;

        public PokerGamesWinLossDetailHandlerTests(UserFixture userFixture)
        {
            reportData = new PagedReportData<PokerGamesWinLossDetailModel>(Enumerable.Empty<PokerGamesWinLossDetailModel>(), 0, 0, 0, default(PokerGamesWinLossDetailModel));
            var reportService = Substitute.For<PokerGamesWinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<PokerGamesWinLossDetailParam>()).Returns(reportData);

            this.userFixture = userFixture;
            winLossDetailHandler = new PokerGamesWinLossDetailHandler(reportService, userFixture.User);
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            PokerGamesWinLossDetailQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandlerWinLossDetailQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new PokerGamesWinLossDetailQuery(userFixture.User);
            message.CustLevelId = 4;

            var winLossDetailParam = new PokerGamesWinLossDetailParam(
                10,
                0,
                0,
                DefaultVariablesTest.CurrentDate,
                DefaultVariablesTest.CurrentDate,
                DateTime.MinValue,
                message.CustLevel,
                "CustName",
                false,
                false);

            var expectedWinLossDetailViewModel = new PokerGamesWinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                new List<CustInfo> { new CustInfo(10, "Name", 4) });

            // Act
            var actualWinLossDetailViewModel = winLossDetailHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedWinLossDetailViewModel, actualWinLossDetailViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new PokerGamesReportLegendQuery();
            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }
    }
}