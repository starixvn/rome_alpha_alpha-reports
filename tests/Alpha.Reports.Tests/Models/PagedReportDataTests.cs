namespace Alpha.Reports.Tests.Models
{
    using System.Linq;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Xunit;

    public class PagedReportDataTests
    {
        [Fact]
        public void CurrentPageIndex_Always_ReturnsCurrentPageIndexFromCtor()
        {
            // Arrange
            var currentPageIndex = 1;
            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), currentPageIndex, 0, 0, default(WinLossDetailModel));

            // Act
            var actual = reportData.CurrentPageIndex;

            // Assert
            Assert.Equal(currentPageIndex, actual);
        }

        [Fact]
        public void PageSize_Always_ReturnsPageSizeFromCtor()
        {
            // Arrange
            var pageSize = 1;
            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, pageSize, 0, default(WinLossDetailModel));

            // Act
            var actual = reportData.PageSize;

            // Assert
            Assert.Equal(pageSize, actual);
        }

        [Fact]
        public void TotalRecords_Always_ReturnsTotalRecordsFromCtor()
        {
            // Arrange
            var totalRecords = 1;
            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, totalRecords, default(WinLossDetailModel));

            // Act
            var actual = reportData.TotalRecords;

            // Assert
            Assert.Equal(totalRecords, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Any_LessThan1Record_ReturnsFalse(int totalRecords)
        {
            // Arrange
            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, totalRecords, default(WinLossDetailModel));

            // Act
            var actual = reportData.Any();

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void Any_AtLeast1Record_ReturnsTrue(int totalRecords)
        {
            // Arrange
            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, totalRecords, default(WinLossDetailModel));

            // Act
            var actual = reportData.Any();

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MoreThanOne_LessThan2Records_ReturnsFalse(int totalRecords)
        {
            // Arrange
            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, totalRecords, default(WinLossDetailModel));

            // Act
            var actual = reportData.MoreThanOne();

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(int.MaxValue)]
        public void MoreThanOne_MoreThan1Record_ReturnsFalse(int totalRecords)
        {
            // Arrange
            var reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, totalRecords, default(WinLossDetailModel));

            // Act
            var actual = reportData.MoreThanOne();

            // Assert
            Assert.True(actual);
        }
    }
}
