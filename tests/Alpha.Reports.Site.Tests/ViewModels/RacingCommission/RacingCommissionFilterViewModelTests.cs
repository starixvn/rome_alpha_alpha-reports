namespace Alpha.Reports.Site.Tests.ViewModels.RacingCommission
{
    using Constants;
    using Reports.Models;
    using Site.ViewModels.RacingCommission;
    using Xunit;

    public class RacingCommissionFilterViewModelTests
    {
        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var racingCommViewModel = new RacingCommissionFilterViewModel(
                1,
                "CustName",
                UserLevel.Agent,
                false,
                DefaultVariablesTest.BalanceUpDate,
                "BreadcrumbLevelJson",
                null);

            // Assert
            Assert.True(
                racingCommViewModel.CustId == 1
                && racingCommViewModel.CustName == "CustName"
                && racingCommViewModel.CustLevel == UserLevel.Agent
                && racingCommViewModel.Visible == false
                && racingCommViewModel.ViewDataInstructionMessage == "You can view the report data from 11/28/2016."
                && racingCommViewModel.DateRangePicker == null
                && racingCommViewModel.BreadcrumbLevelJson == "BreadcrumbLevelJson");
        }
    }
}
