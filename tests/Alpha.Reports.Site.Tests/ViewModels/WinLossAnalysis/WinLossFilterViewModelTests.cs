namespace Alpha.Reports.Site.Tests.ViewModels.WinLossAnalysis
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Reports.Models;
    using Site.ViewModels.WinLossAnalysis;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    public class WinLossAnalysisFilterViewModelTests
    {
        public WinLossAnalysisFilterViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ViewDataInstructionMessage_HistoricReport_ReturnsHistoricNote()
        {
            // Arrange
            var winLossAnalysisFilterViewModel = new WinLossAnalysislFilterViewModel(
                0,
                string.Empty,
                UserLevel.Super,
                null,
                null,
                null,
                false,
                DefaultVariablesTest.CurrentDate,
                Enumerable.Empty<WinLossAnalysisReportTypeViewModel>());

            var expectedViewDataInstructionMessage = "You can view the report data from 12/02/2016.";

            // Act
            var actualViewDataInstructionMessage = winLossAnalysisFilterViewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}