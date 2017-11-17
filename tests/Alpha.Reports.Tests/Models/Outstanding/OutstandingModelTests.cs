namespace Alpha.Reports.Tests.Models.Outstanding
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Outstanding;
    using KellermanSoftware.CompareNetObjects;
    using Xunit;

    [Trait("Category", "Outstanding")]
    public class OutstandingModelTests
    {
        [Fact]
        public void GetValueByLevel_MemberLevel_ReturnsMemberOutstanding()
        {
            // Arrange
            var level = UserLevel.Member;
            var expectedValue = 10;
            var model = new OutstandingModel(0, string.Empty, 10, 0, 0, 0);            

            // Act
            var actualValue = model.GetValueByLevel(level);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetValueByLevel_AgentLevel_ReturnsAgentOutstanding()
        {
            // Arrange
            var level = UserLevel.Agent;
            var expectedValue = 10;
            var model = new OutstandingModel(0, string.Empty, 0, 10, 0, 0);

            // Act
            var actualValue = model.GetValueByLevel(level);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetValueByLevel_MasterLevel_ReturnsMasterOutstanding()
        {
            // Arrange
            var level = UserLevel.Master;
            var expectedValue = 10;
            var model = new OutstandingModel(0, string.Empty, 0, 0, 10, 0);

            // Act
            var actualValue = model.GetValueByLevel(level);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetValueByLevel_SuperLevel_ReturnsSuperOutstanding()
        {
            // Arrange
            var level = UserLevel.Super;
            var expectedValue = 10;
            var model = new OutstandingModel(0, string.Empty, 0, 0, 0, 10);

            // Act
            var actualValue = model.GetValueByLevel(level);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetAccumulatedData_Always_ReturnsCorrectValue()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var outstandings = new List<OutstandingModel>
            {
                CreateOutstandingModel(10, 15, 20, 30),
                CreateOutstandingModel(12, 10, 23, 31),
                CreateOutstandingModel(17, 13, 25, 33)
            };

            var expectedValue = CreateOutstandingModel(39, 38, 68, 94);

            // Act
            var actualValue = OutstandingModel.GetAccumulatedData(outstandings);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualValue).AreEqual);
        }

        private OutstandingModel CreateOutstandingModel(decimal memberValue, decimal agentValue, decimal masterValue, decimal superValue)
            => new OutstandingModel(0, null, memberValue, agentValue, masterValue, superValue);
    }
}
