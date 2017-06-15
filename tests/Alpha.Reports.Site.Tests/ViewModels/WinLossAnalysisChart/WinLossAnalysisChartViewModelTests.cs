namespace Alpha.Reports.Site.Tests.ViewModels.WinLossAnalysisChart
{
    using Constants;
    using Extensions;
    using Fixtures;
    using Params;
    using Reports.Models.WinLossAnalysisChart;
    using Site.Requests.WinLossAnalysisChart;
    using Site.ViewModels.Components;
    using Site.ViewModels.WinLossAnalysisChart;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    [Collection("UserCollection")]
    public class WinLossAnalysisChartViewModelTests
    {
        private readonly UserFixture userFixture;

        public WinLossAnalysisChartViewModelTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void DualAxisChartViewModel_Always_ReturnCorrectViewModel()
        {
            // Arrange
            var reportData = new List<WinLossAnalysisChartModel>
            {
                new WinLossAnalysisChartModel("2016-08", 12051.12m, 56300.16m)
            };

            var message = new WinLossAnalysisChartQuery(userFixture.User);
            var param = new WinLossAnalysisChartParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                Enumerable.Empty<int>(),
                message.ViewerLevel);
            var viewModel = new WinLossAnalysisChartViewModel(reportData, message, param, Enumerable.Empty<SelectListItem>());
            var expectedDualAxisChartViewModel = new DualAxisChartViewModel(
                "Monthly Win Loss Chart From 09/01/2016 To 11/30/2016",
                "Aug-16",
                "Super Win Loss",
                "Turnover",
                "56300.16",
                "12051.12",
                "USS");

            // Act
            var actualDualAxisChartViewModel = viewModel.DualAxisChartViewModel;

            // Assert
            AssertExtension.Equal(expectedDualAxisChartViewModel, actualDualAxisChartViewModel);
        }
    }
}