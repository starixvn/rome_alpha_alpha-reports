namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Alpha.Reports.Models.Winloss;
    using Constants;
    using Enumerations;
    using Extensions;
    using NSubstitute;
    using Reports.Models;
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
        private readonly IUser user;

        public WinLossViewModelTests()
        {
            user = Substitute.For<IUser>();
            winLossViewModel = new WinLossViewModel(new List<WinLossModel>());

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ActionLinkViewModel_NullWinLoss_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossModel nullWinLoss = null;

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
            var winLoss = new WinLossModel();
            winLossViewModel.UserLevel = UserLevel.Super;
            winLossViewModel.FromDate = DefaultVariables.CurrentDate;
            winLossViewModel.ToDate = DefaultVariables.CurrentDate;
            winLossViewModel.ReportData = Enumerable.Empty<WinLossModel>();
            winLossViewModel.BreadcrumbLevels = new List<ActionLinkViewModel>
            {
                new ActionLinkViewModel("ControllerName", "Index", "Link", new WinLossQuery(user))
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
           List<WinLossModel> winlossData,
           WinLossModel expectedGrantTotal)
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
            var expectedGrantTotal = new WinLossModel();

            // Act
            var actualGrantTotal = new WinLossViewModel(null).GrantTotal;

            // Assert
            AssertExtension.Equal(actualGrantTotal, expectedGrantTotal);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedReportData = Enumerable.Empty<WinLossModel>();

            // Act
            var actualReportData = new WinLossViewModel(new List<WinLossModel>()).ReportData;

            // Assert
            AssertExtension.Equal(actualReportData, expectedReportData);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrantTotal = new WinLossModel();

            // Act
            var actualGrantTotal = new WinLossViewModel(new List<WinLossModel>()).GrantTotal;

            // Assert
            AssertExtension.Equal(actualGrantTotal, expectedGrantTotal);
        }
    }
}