namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Params
{
    using System;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Params;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperWinLossDetailByPTParamTests
    {
        [Fact]
        public void Constructor_Always_AssignsCorrectProperties()
        {
            // Arrange
            var fromDate = new DateTime(2021, 7, 1);
            var toDate = new DateTime(2021, 7, 30);
            var userSelectedResourceIds = new[] { 1001 };

            // Act
            var param = new SuperWinLossDetailByPTParam(
                123456,
                fromDate,
                toDate,
                new[] { 1 },
                userSelectedResourceIds,
                string.Empty,
                DateTime.Now);

            // Assert
            Assert.True(fromDate == param.FromDate
                && toDate == param.ToDate
                && userSelectedResourceIds == param.UserSelectedResourceIds);
        }
    }
}
