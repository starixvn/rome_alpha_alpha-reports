namespace Alpha.Reports.Site.Tests.ViewModels.WinLossAnalysis
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.WinLossAnalysis;
    using Site.Requests.WinLossAnalysis;
    using Site.ViewModels.WinLossAnalysis;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    [Collection("UserCollection")]
    public class WinLossAnalysisViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private readonly CompareLogic compareLogic;
        private readonly WinLossAnalysisQuery message;
        private readonly WinLossAnalysisViewModel viewModel;

        public WinLossAnalysisViewModelTests(UserFixture userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.userFixture = userFixture;

            message = CreateWinLossAnalysisQuery(
                            custLevelId: 4,
                            reportTypeId: 1,
                            reportTypeName: "Daily");

            viewModel = new WinLossAnalysisViewModel(
                reportData: Enumerable.Empty<WinLossAnalysisModel>(),
                grandTotal: fixture.Create<WinLossAnalysisModel>(),
                message: message,
                param: fixture.Create<WinLossAnalysisParam>(),
                products: Enumerable.Empty<SelectListItem>());

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void WinLossAnalysisViewModel_NullReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrandTotal = new WinLossAnalysisModel();

            // Act
            var actualGrandTotal = viewModel.GrandTotal;

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossAnalysisViewModel_EmptyReportData_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedReportData = Enumerable.Empty<WinLossAnalysisModel>();

            // Act
            var actualReportData = viewModel.ReportData;

            // Assert
            var result = compareLogic.Compare(actualReportData, expectedReportData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossAnalysisViewModel_EmptyReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrandTotal = new WinLossAnalysisModel();

            // Act
            var actualGrandTotal = viewModel.GrandTotal;

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
        }

        [Theory]
        [InlineData(4, 1, "Daily", "Daily Win Loss Analysis")]
        [InlineData(4, 2, "Monthly", "Monthly Win Loss Analysis")]
        [InlineData(4, 3, "Sport", "Sport Win Loss Analysis")]

        [InlineData(3, 1, "Daily", "Daily Win Loss Analysis")]
        [InlineData(3, 2, "Monthly", "Monthly Win Loss Analysis")]
        [InlineData(3, 3, "Sport", "Sport Win Loss Analysis")]

        [InlineData(2, 1, "Daily", "Daily Win Loss Analysis")]
        [InlineData(2, 2, "Monthly", "Monthly Win Loss Analysis")]
        [InlineData(2, 3, "Sport", "Sport Win Loss Analysis")]
        public void Title_WinLossAnalysisLevel_ReturnCorrectTitle(
            byte custLevelId,
            byte reportTypeId,
            string reportTypeName,
            string expectedTitle)
        {
            // Arrange
            var winLossAnalysisViewModel = new WinLossAnalysisViewModel(
                 reportData: Enumerable.Empty<WinLossAnalysisModel>(),
                 grandTotal: fixture.Create<WinLossAnalysisModel>(),
                 message: CreateWinLossAnalysisQuery(custLevelId, reportTypeId, reportTypeName),
                 param: fixture.Create<WinLossAnalysisParam>(),
                 products: Enumerable.Empty<SelectListItem>());

            // Act & Assert
            Assert.Equal(winLossAnalysisViewModel.Title, expectedTitle);
        }

        [Fact]
        public void SortPropertyName_NotNullSortPropertyName_ReturnCorrectSortPropertyName()
        {
            // Arrange
            var message = new WinLossAnalysisQuery(userFixture.User)
            {
                SortPropertyName = "SortPropertyName"
            };

            var viewModel = new WinLossAnalysisViewModel(null, null, message, null, null);

            // Act
            var actualSortPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal("SortPropertyName", actualSortPropertyName);
        }

        private WinLossAnalysisQuery CreateWinLossAnalysisQuery(byte custLevelId, byte reportTypeId, string reportTypeName)
        {
            var query = new WinLossAnalysisQuery(userFixture.User);
            query.CustLevelId = custLevelId;
            query.ReportTypeId = reportTypeId;
            query.ReportTypeName = reportTypeName;
            query.FromDate = DefaultVariables.CurrentDate;
            query.ToDate = DefaultVariables.CurrentDate;

            return query;
        }
    }
}