namespace Alpha.Reports.Site.Tests.Handlers.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Site.ViewModels.Components;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Reports.Models;
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
            userFixture.User.EnableJackpotProductIds.Returns(new List<int> { 6 });
            compareLogic = new CompareLogic();

            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string> { "Product Name" });
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
                message.LimitedDateToShowInfo,
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
                Enumerable.Empty<GroupProductViewModel>(),
                grandTotal,
                reportData);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public void HandleReportLegendQuery_NotHaveProductsNeedDelayTimeToSumarizeData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.GetOfferedProductsNeedDelayTimeToSumarizeData(message.Site, message.CurrencyId).Returns(Enumerable.Empty<int>());

            var expectedViewModel = new List<string>
            {
                "- The contribution amount will be rounded to 4 decimal places."
            };

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_HaveProductsNeedDelayTimeToSumarizeData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.GetOfferedProductsNeedDelayTimeToSumarizeData(message.Site, message.CurrencyId).Returns(new List<int> { 6 });
            productService.GetAllOfferedProducts(Arg.Any<string>(), Arg.Any<int>()).Returns(new List<Product>
            {
                new Product(6, string.Empty, null, 0, 0)
            });

            var expectedViewModel = new List<string>
            {
                "- The contribution amount will be rounded to 4 decimal places.",
                "- Data of Product Name will be delayed up to 5 minutes after settling tickets."
            };

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }
    }
}
