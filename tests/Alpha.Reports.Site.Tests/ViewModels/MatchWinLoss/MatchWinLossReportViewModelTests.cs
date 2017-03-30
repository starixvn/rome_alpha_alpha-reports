namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Extensions;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.MatchWinLoss;
    using Site.Helpers;
    using Site.Requests.MatchWinLoss;
    using Site.ViewModels.Components;
    using Site.ViewModels.MatchWinLoss;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    public class MatchWinLossReportViewModelTests
    {
        private readonly IUser user;

        public MatchWinLossReportViewModelTests()
        {
            user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);

            UrlHelper.MapExMainPath = (url) => "/ex-main/" + url.Replace("~/", string.Empty);
            UrlHelper.MapBetListPath = (url) => url;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void Title_Always_ReturnsTitleFromResources()
        {
            // Arrange
            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(
                new MatchWinLossQuery(user),
                DateTime.Now,
                new MatchWinLossParam(0, DateTime.Now, DateTime.Now),
                null,
                null);

            var expectedTitle = "Match Win Loss";

            // Act
            var actualTitle = matchWinLossReportViewModel.Title;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void MaxDate_Always_ReturnsMaxValue()
        {
            // Arrange
            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(
                new MatchWinLossQuery(user),
                DateTime.Now,
                new MatchWinLossParam(0, DateTime.Now, DateTime.Now),
                null,
                null);

            var expectedMaxDate = DateTime.MaxValue;

            // Act
            var actualMaxDate = matchWinLossReportViewModel.MaxDate;

            // Assert
            Assert.Equal(expectedMaxDate, actualMaxDate);
        }

        [Fact]
        public void FromDate_Always_ReturnsValueFromParam()
        {
            // Arrange
            var param = new MatchWinLossParam(0, DateTime.Now, DateTime.Now);
            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(
                new MatchWinLossQuery(user),
                DateTime.Now,
                param,
                null,
                null);

            var expectedFromDate = param.FromDate;

            // Act
            var actualFromDate = matchWinLossReportViewModel.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnsValueFromParam()
        {
            // Arrange
            var param = new MatchWinLossParam(0, DateTime.Now, DateTime.Now);
            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(
                new MatchWinLossQuery(user),
                DateTime.Now,
                param,
                null,
                null);

            var expectedToDate = param.ToDate;

            // Act
            var actualToDate = matchWinLossReportViewModel.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void MatchWinLossFilterViewModel_Always_ReturnsCorrectValue()
        {
            // Arrange
            var param = new MatchWinLossParam(0, DateTime.Now, DateTime.Now);
            var query = new MatchWinLossQuery(user);
            var minDate = DateTime.Now;

            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(query, minDate, param, null, null);

            var expectedFilterViewModel = new MatchWinLossFilterViewModel(
                query.CustId,
                query.CustLevel,
                new DateRangePickerViewModel(minDate, DateTime.MaxValue, param.FromDate, param.ToDate, query.UserSelectedDateRangeOption),
                query.IsFilterVisible,
                minDate);

            // Act
            var actualFilterViewModel = matchWinLossReportViewModel.FilterViewModel;

            // Assert
            AssertExtension.Equal(expectedFilterViewModel, actualFilterViewModel);
        }

        [Theory]
        [InlineData(true, "~/WinLossByMatch/Index?MatchId=1&BetType=4&LiveIndicator=1")]
        [InlineData(false, "~/WinLossByMatch/Index?MatchId=1&BetType=4&LiveIndicator=0")]
        public void BuildDrillDownLink_Always_ReturnsBetListLink(bool indicator, string expectedDrillDownLink)
        {
            // Arrange
            var param = new MatchWinLossParam(0, DateTime.Now, DateTime.Now);
            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(
                new MatchWinLossQuery(user),
                DateTime.Now,
                param,
                null,
                null);

            var matchWinLossModel = new MatchWinLossModel(1, 2, 3, 4, indicator, false, "Match Code", "Show Time", 0, 0, 0);

            // Act
            var actualDrillDownLink = matchWinLossReportViewModel.BuildDrillDownLink(
                new MatchWinLossViewModel(string.Empty, string.Empty, matchWinLossModel),
                string.Empty);

            // Assert
            Assert.Equal(expectedDrillDownLink, actualDrillDownLink);
        }

        [Fact]
        public void BuildDrillDownLink_InvalidCustomerLevel_ThrowsNotImplementedException()
        {
            // Arrange
            user.LevelId.Returns((byte)3);
            var param = new MatchWinLossParam(0, DateTime.Now, DateTime.Now);
            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(
                new MatchWinLossQuery(user),
                DateTime.Now,
                param,
                null,
                null);

            var matchWinLossModel = new MatchWinLossModel(1, 2, 3, 4, false, false, "Match Code", "Show Time", 0, 0, 0);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => matchWinLossReportViewModel.BuildDrillDownLink(
                new MatchWinLossViewModel(string.Empty, string.Empty, matchWinLossModel),
                string.Empty));
        }

        [Theory]
        [InlineData(null, "EventInfo")]
        [InlineData("", "EventInfo")]
        [InlineData(" ", "EventInfo")]
        [InlineData("  ", "EventInfo")]
        [InlineData("EventInfo", "EventInfo")]
        [InlineData("Turnover", "Turnover")]
        public void SortPropertyName_CanSort_ReturnsCorrectSortPropertyName(string sortPropertyName, string expectedSortPropertyName)
        {
            // Arrange
            var query = new MatchWinLossQuery(user);
            query.SortPropertyName = sortPropertyName;

            var reportData = new List<MatchWinLossViewModel>
            {
                new MatchWinLossViewModel(string.Empty, string.Empty, 0, 0, false, 0, 0, 0),
                new MatchWinLossViewModel(string.Empty, string.Empty, 0, 0, false, 0, 0, 0)
            };

            var matchWinLossReportViewModel = new MatchWinLossReportViewModel(
                query,
                DateTime.Now,
                new MatchWinLossParam(0, DateTime.Now, DateTime.Now),
                reportData,
                null);

            // Act
            var actualSortPropertyName = matchWinLossReportViewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortPropertyName, actualSortPropertyName);
        }
    }
}
