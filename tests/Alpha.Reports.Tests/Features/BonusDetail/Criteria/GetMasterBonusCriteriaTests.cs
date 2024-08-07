namespace Alpha.Reports.Tests.Features.BonusDetail.Criteria
{
    using System;
    using Alpha.Reports.Features.BonusDetail.Criteria;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Alpha.Reports.Models;
    using Xunit;

    [Trait("Category", "BonusDetail")]
    public class GetMasterBonusCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Master, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(
                param.Username, param.FromDate, param.ToDate, param.StatusId, param.IsComplained, param.PageIndex, param.PageSize, param.ViewerId);
            var expectedSettingKey = "Age_Rpt_BonusDetail_Master";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-8, false)]
        [InlineData(0, false)]
        [InlineData(8, true)]
        public void IsValid_MasterId_ReturnsExpectedValue(int masterId, bool expected)
        {
            // Arrange
            var param = new BonusDetailParam(masterId, UserLevel.Agent, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(
                param.Username, param.FromDate, param.ToDate, param.StatusId, param.IsComplained, param.PageIndex, param.PageSize, param.ViewerId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Master, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(
                param.Username, param.FromDate, param.ToDate, param.StatusId, param.IsComplained, param.PageIndex, param.PageSize, param.ViewerId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsValid_FromDateGreaterThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Master, null, DateTime.Today, DateTime.Today.AddDays(-1), DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(
                param.Username, param.FromDate, param.ToDate, param.StatusId, param.IsComplained, param.PageIndex, param.PageSize, param.ViewerId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }
    }
}
