namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetails
{
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using NSubstitute;
    using Reports.Models;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
    using Site.ViewModels.WinLossDetail;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailsActionLinkViewModelTests
    {
        private readonly IUser user;

        public WinLossDetailsActionLinkViewModelTests()
        {
            user = Substitute.For<IUser>();
        }

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
            var breadcrumbLevels = new List<ActionLinkViewModel>
            {
                new ActionLinkViewModel("ControllerName", "Index", "Link", new WinLossDetailQuery(user))
            };

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
                breadcrumbLevels);

            // Act
            var actualCustId = winLossDetailsActionLinkViewModel.CustId;

            // Assert
            AssertExtension.Equal(expectedCustId, actualCustId);
        }
    }
}