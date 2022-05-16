namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByProduct
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Alpha.Reports.Configurations;
    using Constants;
    using Newtonsoft.Json;
    using Reports.Models;
    using Site.ViewModels.WinLossByProduct;
    using Xunit;

    public class WinLossByProductFilterViewModelTests
    {
        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["SeperateSabaSportsDate"] = "05/17/2022"
            };
            var expectedViewDataInstructionMessage = new List<string>{
                "You can view the report data from 11/28/2016.",
                "The dedicated performance report of SABA Sports is only available from 05/17/2022 onwards. Before 05/17/2022, data is the combination of Sportsbook and SABA Sports."
            };

            var winLossByProductViewModel = new WinLossByProductFilterViewModel(
                1,
                UserLevel.Agent,
                null,
                null,
                null,
                false,
                DefaultVariablesTest.BalanceUpDate);
            // Assert
            Assert.True(
                winLossByProductViewModel.CustId == 1
                && winLossByProductViewModel.CustLevel == UserLevel.Agent
                && winLossByProductViewModel.DateRangePickerViewModel == null
                && winLossByProductViewModel.ProductMultiSelectionViewModel == null
                && winLossByProductViewModel.Visible == false
                && JsonConvert.SerializeObject(winLossByProductViewModel.ViewDataInstructionMessage) == JsonConvert.SerializeObject(expectedViewDataInstructionMessage));
        }
    }
}
