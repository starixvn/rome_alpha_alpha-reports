namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByProduct
{
    using Constants;
    using Reports.Models;
    using Site.ViewModels.WinLossByProduct;
    using Xunit;

    public class WinLossByProductFilterViewModelTests
    {
        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var winLossByProductViewModel = new WinLossByProductFilterViewModel(
                1,
                UserLevel.Agent,
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
                && winLossByProductViewModel.ViewDataInstructionMessage == "You can view the report data from 11/28/2016.");
        }
    }
}
