namespace Alpha.Reports.Site.Tests.Handlers.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Reports.Models.ProgressiveContribution;
    using Services;
    using Site.Handlers.ProgressiveContribution;
    using Site.Requests.ProgressiveContribution;
    using Site.ViewModels.ProgressiveContribution;
    using Xunit;

    [Trait("Category", "ProgressiveGamesContribution")]
    [Collection("UserCollection")]
    public class ProgressiveContributionHandlerTests
    {
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;
        private readonly ProductService productService;
        private readonly ProgressiveContributionService progressiveContributionService;
        private readonly ProgressiveContributionHandler handler;

        public ProgressiveContributionHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            compareLogic = new CompareLogic();

            productService = Substitute.For<ProductService>();
            progressiveContributionService = Substitute.For<ProgressiveContributionService>();

            handler = new ProgressiveContributionHandler(productService, progressiveContributionService);
        }

        [Fact]
        public void Handle_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            ProgressiveContributionQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => handler.Handle(nullMessage));
        }

        [Fact]
        public void Handle_ValidMessage_ReturnCorrectViewModel()
        {
            // Arrange
            var message = new ProgressiveContributionQuery(userFixture.User);

            var param = new ProgressiveContributionParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                Enumerable.Empty<int>(),
                message.CustLevel);

            var reportData = new List<ProgressiveContributionByProductModel>
            {
                new ProgressiveContributionByProductModel(
                    6,
                    "Casino",
                    new List<ProgressiveContributionByBetTypeModel>
                    {
                        new ProgressiveContributionByBetTypeModel(6,2179,string.Empty,2000,100,-40,-30,-20),
                        new ProgressiveContributionByBetTypeModel(6,2320,string.Empty,3000,150,-60,-45,-30),
                        new ProgressiveContributionByBetTypeModel(6,2320,string.Empty,5000,550,-100,-75,-50)
                    })
            };
            progressiveContributionService.GetProgressiveContributions(Arg.Any<ProgressiveContributionParam>()).Returns(reportData);
            var grandTotal = ProgressiveContributionBaseHandler.AccumulateGrandTotal(reportData);
            var expectedViewModel = new ProgressiveContributionViewModel(
                message,
                param,
                Enumerable.Empty<SelectListItem>(),
                grandTotal,
                reportData);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}