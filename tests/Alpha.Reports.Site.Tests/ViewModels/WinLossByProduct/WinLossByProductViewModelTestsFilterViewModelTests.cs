namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByProduct
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Threading;
    using Alpha.Reports.Configurations;
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
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["SeperateSabaSportsDate"] = "05/17/2022"
            };
            var winLossByBetTypeFilterViewModel = new WinLossByProductFilterViewModel(
                0,
                UserLevel.Super,
                null,
                null,
                null,
                false,
                DefaultVariablesTest.CurrentDate);

            var expectedViewDataInstructionMessage = new List<string>{
                "You can view the report data from 12/02/2016.",
                "The dedicated performance report of SABA Sports is only available from 05/17/2022 onwards. Before 05/17/2022, data is the combination of Sportsbook and SABA Sports."
            };

            // Act
            var actualViewDataInstructionMessage = winLossByBetTypeFilterViewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}