namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Enumerations;
    using Models;
    using Site.ViewModels.WinLoss;
    using Xunit;

    public class WinLossFilterViewModelTests
    {
        public WinLossFilterViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ViewDataInstructionMessage_HistoricReport_ReturnsHistoricNote()
        {
            // Arrange
            var winLossFilterViewModel = new WinLossFilterViewModel(
                0,
                string.Empty,
                Enumeration.FromValue<UserLevel>(4),
                true,
                null,
                null,
                null,
                false,
                false,
                DefaultVariables.CurrentDate,
                string.Empty);
            var expectedViewDataInstructionMessage = "You can view the report data from 12/02/2016.";

            // Act
            var actualViewDataInstructionMessage = winLossFilterViewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}
