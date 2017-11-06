namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByProduct
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Reports.Models;
    using Site.ViewModels.WinLossByProduct;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    public class WinLossByProductViewModelTestsFilterViewModelTests
    {
        public WinLossByProductViewModelTestsFilterViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ViewDataInstructionMessage_HistoricReport_ReturnsHistoricNote()
        {
            // Arrange
            var winLossByBetTypeFilterViewModel = new WinLossByProductFilterViewModel(
                0,
                UserLevel.Super,
                null,
                null,
                false,
                DefaultVariablesTest.CurrentDate);

            var expectedViewDataInstructionMessage = "You can view the report data from 12/02/2016.";

            // Act
            var actualViewDataInstructionMessage = winLossByBetTypeFilterViewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}