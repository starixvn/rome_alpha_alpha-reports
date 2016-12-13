namespace Alpha.Reports.Tests.Comparers
{
    using Reports.Models;
    using Reports.Models.WinlossDetails;
    using Xunit;

    public class WinLossDetailReportDataComparerTests
    {
        [Fact]
        public void Equals_CompareNulls_ReturnFalse()
        {
            // Arrange
            ReportData<WinLossDetail> reportA = null;
            ReportData<WinLossDetail> reportB = null;

            // Act
            var actualEqual = WinLossDetailReportDataComparer.Equals(reportA, reportB);

            // Assert
            Assert.True(actualEqual);
        }

        [Fact]
        public void Equals_OnlyWReportDataAIsNull_ReturnFalse()
        {
            // Arrange
            ReportData<WinLossDetail> reportA = null;
            var reportB = new ReportData<WinLossDetail>(null, 0, 0, 0, null);

            // Act
            var actualEqual = WinLossDetailReportDataComparer.Equals(reportA, reportB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_OnlyReportDataBIsNull_ReturnFalse()
        {
            // Arrange
            var reportA = new ReportData<WinLossDetail>(null, 0, 0, 0, null);
            ReportData<WinLossDetail> reportB = null;

            // Act
            var actualEqual = WinLossDetailReportDataComparer.Equals(reportA, reportB);

            // Assert
            Assert.False(actualEqual);
        }
    }
}
