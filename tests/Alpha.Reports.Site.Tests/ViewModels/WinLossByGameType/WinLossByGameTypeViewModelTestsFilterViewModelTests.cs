namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByGameType
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Reports.Models;
    using Site.ViewModels.WinLossByGameType;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    public class WinLossByGameTypeViewModelTestsFilterViewModelTests
    {
        public WinLossByGameTypeViewModelTestsFilterViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ViewDataInstructionMessage_HistoricReport_ReturnsHistoricNote()
        {
            // Arrange
            var winLossByGameTypeFilterViewModel = new WinLossByGameTypeFilterViewModel(
                0,
                UserLevel.Super,
                null,
                false,
                DefaultVariables.CurrentDate);

            var expectedViewDataInstructionMessage = "You can view the report data from 12/02/2016.";

            // Act
            var actualViewDataInstructionMessage = winLossByGameTypeFilterViewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}