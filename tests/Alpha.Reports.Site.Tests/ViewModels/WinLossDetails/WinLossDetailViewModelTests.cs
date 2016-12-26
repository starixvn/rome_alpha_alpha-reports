namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetails
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Enumerations;
    using Extensions;
    using Models;
    using Models.WinlossDetails;
    using NSubstitute;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels;
    using Site.ViewModels.WinLossDetails;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    public class WinLossDetailViewModelTests
    {
        private const int AllBetProductId = 21;
        private readonly WinLossDetailViewModel winLossDetailViewModel;
        private readonly IUserSession userSession;

        public WinLossDetailViewModelTests()
        {
            winLossDetailViewModel = new WinLossDetailViewModel();
            userSession = Substitute.For<IUserSession>();

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
        public void ActionLinkViewModel_NullWinLossDetail_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetail nullWinLossDetail = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailViewModel.ActionLinkViewModel(nullWinLossDetail));
        }

        [Fact]
        public void ActionLinkViewModel_NotNullWinLossDetail_ReturnsCorrectActionLinkViewModel()
        {
            // Arrange
            var winLossDetail = new WinLossDetail();
            winLossDetailViewModel.UserLevel = Enumeration.FromValue<UserLevel>(4);
            winLossDetailViewModel.FromDate = DefaultVariables.CurrentDate;
            winLossDetailViewModel.ToDate = DefaultVariables.CurrentDate;
            winLossDetailViewModel.ReportData = new ReportData<WinLossDetail>(null, 0, 0, 0, default(WinLossDetail));
            winLossDetailViewModel.BreadcrumbLevels = new List<ActionLinkViewModel>
            {
                new ActionLinkViewModel("ControllerName", "Index", "Link", new WinLossDetailQuery(userSession))
            };

            var expectedActionLinkViewModel = new WinLossDetailsActionLinkViewModel(
                    0,
                    null,
                    3,
                    DefaultVariables.CurrentDate,
                    DefaultVariables.CurrentDate,
                    0,
                    null,
                    null,
                    false,
                    false,
                    false,
                    false,
                    0,
                    winLossDetailViewModel.BreadcrumbLevels);

            // Act
            var actualActionLinkViewModel = winLossDetailViewModel.ActionLinkViewModel(winLossDetail);

            // Assert
            AssertExtension.Equal(expectedActionLinkViewModel, actualActionLinkViewModel);
        }
    }
}
