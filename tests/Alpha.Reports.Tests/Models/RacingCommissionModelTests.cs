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
            var model = new RacingCommissionModel(0, string.Empty, 1, 2, 3);
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
                new RacingCommissionModel(0, string.Empty, 1, 2, 3),
                new RacingCommissionModel(0, string.Empty, 4, 5, 6)
            };

            var expectedCommission = new RacingCommissionModel(0, string.Empty, 5, 7, 9);

            // Act
            var accumulatedCommission = RacingCommissionModel.GetAccumulatedCommission(commissions);

            // Assert
            Assert.True(new CompareLogic().Compare(expectedCommission, accumulatedCommission).AreEqual);
        }
    }
}