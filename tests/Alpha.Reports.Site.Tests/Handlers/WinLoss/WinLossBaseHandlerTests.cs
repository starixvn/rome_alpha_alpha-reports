namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using System.Collections.Generic;
    using Extensions;
    using NSubstitute;
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossBaseHandlerTests
    {
        private readonly WinLossBaseHandler handler;

        public WinLossBaseHandlerTests()
        {
            var productService = Substitute.For<ProductService>();
            var productDetailService = Substitute.For<ProductDetailService>();
            var reportService = Substitute.For<WinLossService>();
            handler = new WinLossBaseHandler(productService, productDetailService, reportService);
        }

        [Fact]
        public void AccumulateGrandTotal_HasData_ReturnsCorrectGrandTotal()
        {
            // Arrange
            var reportData = new List<WinLossModel>
            {
                new WinLossModel(1, "CustName", 1, 1, null, null, null, null, false)
            };

            var expectedGrandTotal = new WinLossModel(0, null, 1, 1, null, null, null, null, false);

            // Act
            var actualGrandTotal = handler.AccumulateGrandTotal(reportData);

            // Assert
            AssertExtension.Equal(expectedGrandTotal, actualGrandTotal);
        }

        [Fact]
        public void AccumulateGrandTotal_NullData_ReturnsCorrectGrandTotal()
        {
            // Arrange
            IEnumerable<WinLossModel> reportData = null;

            var expectedGrandTotal = new WinLossModel();

            // Act
            var actualGrandTotal = handler.AccumulateGrandTotal(reportData);

            // Assert
            AssertExtension.Equal(expectedGrandTotal, actualGrandTotal);
        }
    }
}
