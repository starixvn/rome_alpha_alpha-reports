namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    [Collection("UserCollection")]
    public class WinLossDetailViewModelTests
    {
        private const int AllBetProductId = 21;
        private readonly UserFixture userFixture;

        public WinLossDetailViewModelTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void NetTurnoverTooltips_ProductsNotContainAllBet_ReturnsEmptyList()
        {
            // Arrange
            var message = new WinLossDetailQuery(userFixture.User);
            var winLossDetailViewModel = new WinLossDetailViewModel(
                message,
                new WinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), message.CustLevel, "CustName", false, false),
                new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());
            var expectedNetTurnoverTooltips = new List<string>();

            // Act
            var actualNetTurnoverTooltips = winLossDetailViewModel.NetTurnoverTooltips;

            // Assert
            Assert.Equal(expectedNetTurnoverTooltips, actualNetTurnoverTooltips);
        }

        [Fact]
        public void NetTurnoverTooltips_ProductsContainsAllBet_ReturnsValueContainsAllBetTooltip()
        {
            // Arrange
            var message = new WinLossDetailQuery(userFixture.User);
            var winLossDetailViewModel = new WinLossDetailViewModel(
                message,
                new WinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), message.CustLevel, "CustName", false, false),
                new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                 new List<SelectListItem>
                 {
                     new SelectListItem { Value = AllBetProductId.ToString() }
                 });

            var expectedNetTurnoverTooltips = new List<string> { "- For AllBet, Net Turnover is total valid bets that we use to compute commission." };

            // Act
            var actualNetTurnoverTooltips = winLossDetailViewModel.NetTurnoverTooltips;

            // Assert
            Assert.Equal(expectedNetTurnoverTooltips, actualNetTurnoverTooltips);
        }
    }
}