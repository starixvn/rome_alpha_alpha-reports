namespace Alpha.Reports.Tests.Comparers
{
    using System.Collections.Generic;
    using Reports.Models.WinlossDetails;
    using Xunit;

    public class WinLossDetailComparerTests
    {
        [Fact]
        public void Equals_DifferentList_ReturnFalse()
        {
            // Arrange
            var winLossDetailA = new List<WinLossDetail>()
            {
                new WinLossDetail(0, string.Empty, 0, 0, 0, 0, null, null, null, null)
            };

            var winLossDetailB = new List<WinLossDetail>()
            {
                new WinLossDetail(1, string.Empty, 0, 0, 0, 0, null, null, null, null)
            };

            // Act
            var actualEqual = WinLossDetailComparer.Equals(winLossDetailA, winLossDetailB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_OnlyWinLossDetailAIsNull_ReturnFalse()
        {
            // Arrange
            WinLossDetail winLossDetailA = null;
            var winLossDetailB = new WinLossDetail();

            // Act
            var actualEqual = WinLossDetailComparer.Equals(winLossDetailA, winLossDetailB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_OnlyWinLossDetailBIsNull_ReturnFalse()
        {
            // Arrange
            var winLossDetailA = new WinLossDetail();
            WinLossDetail winLossDetailB = null;

            // Act
            var actualEqual = WinLossDetailComparer.Equals(winLossDetailA, winLossDetailB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_CompareNulls_ReturnTrue()
        {
            // Arrange
            WinLossDetail winLossDetailA = null;
            WinLossDetail winLossDetailB = null;

            // Act
            var actualEqual = WinLossDetailComparer.Equals(winLossDetailA, winLossDetailB);

            // Assert
            Assert.True(actualEqual);
        }

        [Fact]
        public void Equals_NullLists_ReturnTrue()
        {
            // Arrange
            List<WinLossDetail> winLossDetailA = null;

            List<WinLossDetail> winLossDetailB = null;

            // Act
            var actualEqual = WinLossDetailComparer.Equals(winLossDetailA, winLossDetailB);

            // Assert
            Assert.True(actualEqual);
        }
    }
}
