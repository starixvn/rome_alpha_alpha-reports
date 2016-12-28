namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Alpha.Reports.Models.Winloss;
    using Constants;
    using Enumerations;
    using Extensions;
    using Models;
    using NSubstitute;
    using Reports.Tests.TestCases;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Site.ViewModels.WinLoss;
    using Sunplus.Agent.Model.Entities;
    using TestCases;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossViewModelTests
    {
        private readonly WinLossViewModel winLossViewModel;
        private readonly IUserSession userSession;

        public WinLossViewModelTests()
        {
            userSession = Substitute.For<IUserSession>();
            winLossViewModel = new WinLossViewModel(new List<WinLoss>());

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ActionLinkViewModel_NullWinLoss_ThrowsArgumentNullException()
        {
            // Arrange
            WinLoss nullWinLoss = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossViewModel.ActionLinkViewModel(nullWinLoss));
        }

        [Theory]
        [InlineData(4, "Master Win Loss")]
        [InlineData(3, "Agent Win Loss")]
        [InlineData(2, "Member Win Loss")]
        public void Title_WinLossLevel_ReturnCorrectTitle(
            byte userLevel,
            string expectedTitle)
        {
            // Arrange
            winLossViewModel.UserLevel = Enumeration.FromValue<UserLevel>(userLevel);

            // Act & Assert
            Assert.Equal(winLossViewModel.Title, expectedTitle);
        }

        [Fact]
        public void ActionLinkViewModel_NotNullWinLoss_ReturnsCorrectActionLinkViewModel()
        {
            // Arrange
            var winLoss = new WinLoss();
            winLossViewModel.UserLevel = UserLevel.Super;
            winLossViewModel.FromDate = DefaultVariables.CurrentDate;
            winLossViewModel.ToDate = DefaultVariables.CurrentDate;
            winLossViewModel.ReportData = Enumerable.Empty<WinLoss>();
            winLossViewModel.BreadcrumbLevels = new List<ActionLinkViewModel>
            {
                new ActionLinkViewModel("ControllerName", "Index", "Link", new WinLossQuery(userSession))
            };

            var expectedActionLinkViewModel = new WinLossActionLinkViewModel(
                    0,
                    null,
                    3,
                    DefaultVariables.CurrentDate,
                    DefaultVariables.CurrentDate,
                    0,
                    null,
                    false,
                    false,
                    false,
                    winLossViewModel.BreadcrumbLevels);

            // Act
            var actualActionLinkViewModel = winLossViewModel.ActionLinkViewModel(winLoss);

            // Assert
            AssertExtension.Equal(expectedActionLinkViewModel, actualActionLinkViewModel);
        }

        [Theory]
        [MemberData("WinLossViewModelSources", MemberType = typeof(WinLossViewModelTestCases))]
        public void WinLossViewModel_ValidReportData_ReturnsCorrectGrandTotal(
           List<WinLoss> winlossData,
           WinLoss expectedGrantTotal)
        {
            // Arrange

            // Act
            var actualGrantTotal = new WinLossViewModel(winlossData).GrantTotal;

            // Assert
            AssertExtension.Equal(actualGrantTotal, expectedGrantTotal);
        }

        [Fact]
        public void WinLossViewModel_NullReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange 
            var expectedGrantTotal = new WinLoss();

            // Act
            var actualGrantTotal = new WinLossViewModel(null).GrantTotal;

            // Assert
            AssertExtension.Equal(actualGrantTotal, expectedGrantTotal);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange 
            var expectedGrantTotal = new WinLoss();

            // Act
            var actualGrantTotal = new WinLossViewModel(new List<WinLoss>()).GrantTotal;

            // Assert
            AssertExtension.Equal(actualGrantTotal, expectedGrantTotal);
        }
    }
}