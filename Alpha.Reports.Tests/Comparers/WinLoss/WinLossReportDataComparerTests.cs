namespace Alpha.Reports.Tests.Comparers.WinLoss
{
    using Reports.Models;
    using Reports.Models.Winloss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossReportDataComparerTests
    {
        [Fact]
        public void Equals_CompareNulls_ReturnFalse()
        {
            // Arrange
            ReportData<WinLoss> reportA = null;
            ReportData<WinLoss> reportB = null;

            // Act
            var actualEqual = WinLossReportDataComparer.Equals(reportA, reportB);

            // Assert
            Assert.True(actualEqual);
        }

        [Fact]
        public void Equals_OnlyWReportDataAIsNull_ReturnFalse()
        {
            // Arrange
            ReportData<WinLoss> reportA = null;
            var reportB = new ReportData<WinLoss>(null, 0, null);

            // Act
            var actualEqual = WinLossReportDataComparer.Equals(reportA, reportB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_OnlyReportDataBIsNull_ReturnFalse()
        {
            // Arrange
            var reportA = new ReportData<WinLoss>(null, 0, 0, 0, null);
            ReportData<WinLoss> reportB = null;

            // Act
            var actualEqual = WinLossReportDataComparer.Equals(reportA, reportB);

            // Assert
            Assert.False(actualEqual);
        }
    }
}
