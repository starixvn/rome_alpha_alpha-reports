namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetails
{
    using Constants;
    using Extensions;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailsActionLinkViewModelTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void HasStackIcon_Always_ReturnsHasStackIconSetFromConstructor(int custId)
        {
            // Arrange
            var expectedCustId = custId;

            var winLossDetailsActionLinkViewModel = new WinLossDetailsActionLinkViewModel(
                custId,
                string.Empty,
                0,
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                0,
                string.Empty,
                string.Empty,
                false,
                false,
                false,
                false,
                0,
                 "[{'CustId':1,'CustName':'SUPER1','UserLevelId':4}]");

            // Act
            var actualCustId = winLossDetailsActionLinkViewModel.CustId;

            // Assert
            AssertExtension.Equal(expectedCustId, actualCustId);
        }
    }
}