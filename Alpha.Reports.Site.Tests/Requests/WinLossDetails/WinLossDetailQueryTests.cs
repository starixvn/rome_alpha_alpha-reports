namespace Alpha.Reports.Site.Tests.Requests.WinLossDetails
{
    using Extensions;
    using Site.Requests.WinLossDetails;
    using Xunit;

    public class WinLossDetailQueryTests
    {
        [Fact]
        public void UseDefaultConstructor_Always_ReturnsDefaultInstance()
        {
            // Arrange
            var expectedWinLossDetailQuery = new WinLossDetailQuery
            {
                UserSelectedProductIds = null,
                SortPropertyName = null,
                IsDescendingSort = true,
                IsStackMode = false,
                IsHistoricReport = false,
                IsFilterVisible = false,
                UserSelectedDateRangeOption = 0
            };

            // Act
            var actualWinLossDetailQuery = new WinLossDetailQuery();

            // Assert
            AssertExtension.Equal(expectedWinLossDetailQuery, actualWinLossDetailQuery);
        }
    }
}
