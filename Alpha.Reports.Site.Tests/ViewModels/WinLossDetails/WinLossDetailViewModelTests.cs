namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetails
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Enumerations;
    using Extensions;
    using Models;
    using Models.WinlossDetails;
    using Site.ViewModels.WinLossDetails;
    using Xunit;

    public class WinLossDetailViewModelTests
    {
        private const int AllBetProductId = 21;
        private readonly WinLossDetailViewModel winLossDetailViewModel;

        public WinLossDetailViewModelTests()
        {
            winLossDetailViewModel = new WinLossDetailViewModel();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void NetTurnoverTooltips_ProductsNotContainAllBet_ReturnsEmptyList()
        {
            // Arrange
            winLossDetailViewModel.Products = new List<SelectListItem>();
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
            winLossDetailViewModel.Products = new List<SelectListItem>
            {
                new SelectListItem { Value = AllBetProductId.ToString() }
            };

            var expectedNetTurnoverTooltips = new List<string> { "- For AllBet, Net Turnover is total valid bets that we use to compute commission." };

            // Act
            var actualNetTurnoverTooltips = winLossDetailViewModel.NetTurnoverTooltips;

            // Assert
            Assert.Equal(expectedNetTurnoverTooltips, actualNetTurnoverTooltips);
        }

        [Fact]
        public void LinkActionViewModel_NullWinLossDetail_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetail nullWinLossDetail = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailViewModel.LinkActionViewModel(nullWinLossDetail));
        }
    }
}
