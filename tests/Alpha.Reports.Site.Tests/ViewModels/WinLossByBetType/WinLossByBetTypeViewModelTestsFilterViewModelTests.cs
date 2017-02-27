namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByBetType
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Reports.Models;
    using Site.ViewModels.WinLossByBetType;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    public class WinLossByBetTypeViewModelTestsFilterViewModelTests
    {
        public WinLossByBetTypeViewModelTestsFilterViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ViewDataInstructionMessage_HistoricReport_ReturnsHistoricNote()
        {
            // Arrange
            var winLossByBetTypeFilterViewModel = new WinLossByBetTypeFilterViewModel(
                0,
                UserLevel.Super,
                null,
                null,
                false,
                DefaultVariables.CurrentDate);

            var expectedViewDataInstructionMessage = "You can view the report data from 12/02/2016.";

            // Act
            var actualViewDataInstructionMessage = winLossByBetTypeFilterViewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}