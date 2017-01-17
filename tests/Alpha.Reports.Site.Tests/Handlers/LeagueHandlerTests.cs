namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Extensions;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Services;
    using Site.Handlers;
    using Site.Requests;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    public class LeagueHandlerTests
    {
        private readonly LeagueService leagueService;
        private readonly LeagueHandler leagueHandler;
        private readonly IUser viewer;

        public LeagueHandlerTests()
        {
            leagueService = Substitute.For<LeagueService>();
            leagueHandler = new LeagueHandler(leagueService);

            viewer = Substitute.For<IUser>();
            viewer.LevelId.Returns((byte)1);
        }

        [Fact]
        public void HandleLeaguesByCustomerQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            LeaguesByCustomerQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => leagueHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleLeaguesByCustomerQuery_NotNullMessage_CallsLeagueService()
        {
            // Arrange
            var message = new LeaguesByCustomerQuery(viewer);

            // Act 
            leagueHandler.Handle(message);

            // Assert
            leagueService.Received().GetLeaguesByCustomer(Arg.Is<GetLeaguesByCustomerParam>(
                param => param.CustId == message.CustId
                && param.Products == message.Products
                && param.FromDate == message.FromDate
                && param.ToDate == message.ToDate
                && param.Language == message.Language));
        }

        [Fact]
        public void HandleLeaguesByCustomerQuery_NotNullMessage_ReturnsDataFromLeagueService()
        {
            // Arrange
            var message = new LeaguesByCustomerQuery(viewer);

            var leagues = new List<League>
            {
                new League(1, "League 1", 1),
                new League(2, "League 2", 2)
            };

            var expectedLeaguesByCustomerViewModel = new LeaguesByCustomerViewModel(
                string.Empty,
                new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Selected = true },
                    new SelectListItem { Value = "2", Selected = true }
                });

            leagueService.GetLeaguesByCustomer(Arg.Any<GetLeaguesByCustomerParam>()).Returns(leagues);

            // Act 
            var actualLeaguesByCustomerViewModel = leagueHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedLeaguesByCustomerViewModel, actualLeaguesByCustomerViewModel);
        }
    }
}
