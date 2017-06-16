namespace Alpha.Reports.Tests.Params
{
    using Reports.Params;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class DailyEnteredSportParamTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsShowTestData_Always_ReturnsValueSetFromConstructor(bool isShowTestData)
        {
            // Arrange
            var param = new DailyEnteredSportParam(isShowTestData);
            var expectedIsShowTestData = isShowTestData;

            // Act
            var actualIsShowTestData = param.IsShowTestData;

            // Assert
            Assert.Equal(expectedIsShowTestData, actualIsShowTestData);
        }
    }
}
