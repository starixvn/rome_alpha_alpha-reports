namespace Alpha.Reports.Site.Tests.ViewModels.CommissionByBettype
{
    using Constants;
    using Reports.Models;
    using Site.ViewModels.CommissionByBettype;
    using Xunit;

    public class CommissionByBettypeFilterViewModelTests
    {
        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var viewModel = new CommissionByBettypeFilterViewModel(
                1,
                "CustName",
                UserLevel.Agent,
                false,
                DefaultVariablesTest.BalanceUpDate,
                "BreadcrumbLevelJson",
                null);

            // Assert
            Assert.True(
                viewModel.CustId == 1
                && viewModel.CustName == "CustName"
                && viewModel.CustLevel == UserLevel.Agent
                && viewModel.Visible == false
                && viewModel.ViewDataInstructionMessage == "You can view the report data from 11/28/2016."
                && viewModel.DateRangePicker == null
                && viewModel.BreadcrumbLevelJson == "BreadcrumbLevelJson");
        }
    }
}
