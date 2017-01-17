namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetail
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Reports.Models;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailViewModelTests
    {
        private const int AllBetProductId = 21;
        private readonly WinLossDetailViewModel winLossDetailViewModel;

        public WinLossDetailViewModelTests()
        {
            winLossDetailViewModel = new WinLossDetailViewModel(Enumerable.Empty<CustInfo>());

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

        ////[Fact]
        ////public void ActionLinkViewModel_NullWinLossDetail_ThrowsArgumentNullException()
        ////{
        ////    // Arrange
        ////    WinLossDetailModel nullWinLossDetail = null;

        ////    // Act & Assert
        ////    Assert.Throws<ArgumentNullException>(() => winLossDetailViewModel.ActionLinkViewModel(nullWinLossDetail));
        ////}

        ////[Fact]
        ////public void ActionLinkViewModel_NotNullWinLossDetail_ReturnsCorrectActionLinkViewModel()
        ////{
        ////    // Arrange
        ////    var winLossDetail = new WinLossDetailModel();
        ////    winLossDetailViewModel.CustLevel = Enumeration.FromValue<UserLevel>(4);
        ////    winLossDetailViewModel.FromDate = DefaultVariables.CurrentDate;
        ////    winLossDetailViewModel.ToDate = DefaultVariables.CurrentDate;
        ////    winLossDetailViewModel.ReportData = new ReportData<WinLossDetailModel>(null, 0, 0, 0, default(WinLossDetailModel));
        ////    winLossDetailViewModel.BreadcrumbLevels = new List<ActionLinkViewModel>
        ////    {
        ////        new ActionLinkViewModel("ControllerName", "Index", "Link", new WinLossDetailQuery(user))
        ////    };
        ////    winLossDetailViewModel.DrillDownInfoJson = "[{'CustId':1,'CustName':'SUPER1','CustLevelId':4}]";

        ////    var expectedActionLinkViewModel = new WinLossDetailsActionLinkViewModel(
        ////            0,
        ////            null,
        ////            3,
        ////            DefaultVariables.CurrentDate,
        ////            DefaultVariables.CurrentDate,
        ////            0,
        ////            null,
        ////            null,
        ////            false,
        ////            false,
        ////            false,
        ////            false,
        ////            0,
        ////             "[{'CustId':1,'CustName':'SUPER1','CustLevelId':4}]");

        ////    // Act
        ////    var actualActionLinkViewModel = winLossDetailViewModel.ActionLinkViewModel(winLossDetail);

        ////    // Assert
        ////    Assert.True(compareLogic.Compare(expectedActionLinkViewModel, actualActionLinkViewModel).AreEqual);
        ////}
    }
}