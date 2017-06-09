using Alpha.Reports.Params;
using Xunit;

namespace Alpha.Reports.Tests.Params
{
    public class DailyEnteredSportParamTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsShowTestData_Always_ReturnsDataSetFromConstructor(bool isShowTestData)
        {
            // Arrange
            var expectedIsShowTestData = isShowTestData;
            var param = new DailyEnteredSportParam(isShowTestData);

            // Act
            var actualIsShowTestData = param.IsShowTestData;

            // Assert
            Assert.Equal(expectedIsShowTestData, actualIsShowTestData);
        }
    }
}
