namespace Alpha.Reports.Tests.Models
{
    using System.Collections.Generic;
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models.RacingCommission;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    public class RacingCommissionModelTests
    {
        [Fact]
        public void Total_WhenAcceses_ReturnsSumAllToteProperties()
        {
            // Arrange
            var model = new RacingCommissionModel
            {
                ToteWin = 1,
                TotePlace = 2,
                ToteWinPlace = 3
            };
            var expectedValue = model.ToteWin + model.TotePlace + model.ToteWinPlace;

            // Act

            // Assert
            Assert.Equal(expectedValue, model.Total);
        }

        [Fact]
        public void GetAccumulatedCommission_WhenAcceses_ReturnsSumAllToteProperties()
        {
            // Arrange
            var commissions = new List<RacingCommissionModel>
            {
                  new RacingCommissionModel { ToteWin = 1, TotePlace = 2, ToteWinPlace = 3 },
                  new RacingCommissionModel { ToteWin = 4, TotePlace = 5, ToteWinPlace = 6 }
            };

            var expectedCommission = new RacingCommissionModel { ToteWin = 5, TotePlace = 7, ToteWinPlace = 9 };

            // Act
            var accumulatedCommission = RacingCommissionModel.GetAccumulatedCommission(commissions);

            // Assert
            Assert.True(new CompareLogic().Compare(expectedCommission, accumulatedCommission).AreEqual);
        }
    }
}