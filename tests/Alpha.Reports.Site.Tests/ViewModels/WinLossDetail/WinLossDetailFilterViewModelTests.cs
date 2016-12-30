namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetails
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Enumerations;
    using Models;
    using Reports.Models;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    public class WinLossDetailFilterViewModelTests
    {
        public WinLossDetailFilterViewModelTests()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void ViewDataInstructionMessage_HistoricReport_ReturnsHistoricNote()
        {
            // Arrange
            var winLossDetailFilterViewModel = new WinLossDetailFilterViewModel(
                0,
                string.Empty,
                Enumeration.FromValue<UserLevel>(4),
                true,
                null,
                null,
                null,
                false,
                DefaultVariables.CurrentDate,
                string.Empty);
            var expectedViewDataInstructionMessage = "You can view the report data from 12/02/2016.";

            // Act
            var actualViewDataInstructionMessage = winLossDetailFilterViewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}