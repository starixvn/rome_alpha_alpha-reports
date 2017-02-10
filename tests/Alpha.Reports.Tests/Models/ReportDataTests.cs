namespace Alpha.Reports.Tests.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Xunit;

    public class ReportDataTests
    {
        [Fact]
        public void DataSource_NullDataSource_ReturnsEmptyWinLossDetailListFromCtor()
        {
            // Arrange
            var reportData = new ReportData<WinLossDetailModel>(null, default(WinLossDetailModel));
            var expectedDataSource = Enumerable.Empty<WinLossDetailModel>();

            // Act
            var actual = reportData.DataSource;

            // Assert
            Assert.Equal(expectedDataSource, actual);
        }

        [Fact]
        public void DataSource_NotNullDataSource_ReturnsDataSourceFromCtor()
        {
            // Arrange
            var dataSource = Enumerable.Empty<WinLossDetailModel>();
            var reportData = new ReportData<WinLossDetailModel>(dataSource, default(WinLossDetailModel));

            // Act
            var actual = reportData.DataSource;

            // Assert
            Assert.Equal(dataSource, actual);
        }

        [Fact]
        public void TotalData_Always_ReturnsTotalDataFromCtor()
        {
            // Arrange
            var totalData = new WinLossDetailModel();
            var reportData = new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), totalData);

            // Act
            var actual = reportData.TotalData;

            // Assert
            Assert.Equal(totalData, actual);
        }

        [Fact]
        public void Any_LessThan1Record_ReturnsFalse()
        {
            // Arrange
            var reportData = new ReportData<string>(Enumerable.Empty<string>(), string.Empty);

            // Act
            var actual = reportData.Any();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Any_AtLeast1Record_ReturnsTrue()
        {
            // Arrange
            var reportData = new ReportData<string>(new List<string> { "1" }, string.Empty);

            // Act
            var actual = reportData.Any();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void MoreThanOne_LessThan2Records_ReturnsFalse()
        {
            // Arrange
            var reportData = new ReportData<string>(new List<string> { "1" }, string.Empty);

            // Act
            var actual = reportData.MoreThanOne();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void MoreThanOne_MoreThan1Record_ReturnsFalse()
        {
            // Arrange
            var reportData = new ReportData<string>(new List<string> { "1", "2" }, string.Empty);

            // Act
            var actual = reportData.MoreThanOne();

            // Assert
            Assert.True(actual);
        }
    }
}