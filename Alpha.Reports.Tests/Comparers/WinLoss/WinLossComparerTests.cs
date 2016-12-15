namespace Alpha.Reports.Tests.Comparers.WinLoss
{
    using System.Collections.Generic;
    using Reports.Models.Winloss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossComparerTests
    {
        [Fact]
        public void Equals_DifferentList_ReturnFalse()
        {
            // Arrange
            var winLossA = new List<WinLoss>()
            {
                new WinLoss(0, string.Empty, 0, 0, null, null, null, null)
            };

            var winLossB = new List<WinLoss>()
            {
                new WinLoss(1, string.Empty, 0, 0, null, null, null, null)
            };

            // Act
            var actualEqual = WinLossComparer.Equals(winLossA, winLossB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_OnlyWinLossAIsNull_ReturnFalse()
        {
            // Arrange
            WinLoss winLossA = null;
            var winLossB = new WinLoss();

            // Act
            var actualEqual = WinLossComparer.Equals(winLossA, winLossB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_OnlyWinLossBIsNull_ReturnFalse()
        {
            // Arrange
            var winLossA = new WinLoss();
            WinLoss winLossB = null;

            // Act
            var actualEqual = WinLossComparer.Equals(winLossA, winLossB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_CompareNulls_ReturnTrue()
        {
            // Arrange
            WinLoss winLossA = null;
            WinLoss winLossB = null;

            // Act
            var actualEqual = WinLossComparer.Equals(winLossA, winLossB);

            // Assert
            Assert.True(actualEqual);
        }

        [Fact]
        public void Equals_NullLists_ReturnTrue()
        {
            // Arrange
            List<WinLoss> winLossA = null;

            List<WinLoss> winLossB = null;

            // Act
            var actualEqual = WinLossComparer.Equals(winLossA, winLossB);

            // Assert
            Assert.True(actualEqual);
        }
    }
}
