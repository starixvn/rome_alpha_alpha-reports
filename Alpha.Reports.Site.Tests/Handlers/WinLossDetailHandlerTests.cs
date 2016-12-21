namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Models.WinlossDetails;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLossDetails;
    using Services;
    using Site.Handlers;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    public class WinLossDetailHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;

        private readonly Workbook<WinLossDetail> workbook;
        private readonly WinLossDetailHandler winLossDetailHandler;
        private readonly IUserSession userSession;

        public WinLossDetailHandlerTests()
        {
            productService = Substitute.For<ProductService>();
            reportService = Substitute.For<WinLossDetailService>();
            workbook = Substitute.For<Workbook<WinLossDetail>>();
            userSession = Substitute.For<IUserSession>();
            winLossDetailHandler = new WinLossDetailHandler(productService, reportService, workbook, userSession);
        }

        [Fact]
        public void HandlerWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailHandler.Handle(nullMessage));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void HandlerWinLossDetailQuery_NullOrWhiteSpaceUserSelectedProductIds_ThrowsArgumentNullException2(
            string userSelectedProductIds)
        {
            // Arrange
            var message = new WinLossDetailQuery(userSession);
            message.UserLevelId = 4;
            message.UserSelectedProductIds = userSelectedProductIds;

            var products = Enumerable.Empty<Product>();
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(products);

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            ReportData<WinLossDetail> reportData = null;
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            // Act
            var actualWinLossDetailViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(actualWinLossDetailViewModel, actualWinLossDetailViewModel);
        }
    }
}
