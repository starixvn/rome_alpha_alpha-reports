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
    using Models.WinlossDetails;
    using NSubstitute;
    using Site.Requests.WinLoss;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels;
    using Site.ViewModels.WinLoss;
    using Site.ViewModels.WinLossDetails;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossViewModelTests
    {
        private const int AllBetProductId = 21;
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
        public void Title_MasterLevel_ReturnCorrectTitle(
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
            winLossViewModel.UserLevel = Enumeration.FromValue<UserLevel>(4);
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
    }
}
