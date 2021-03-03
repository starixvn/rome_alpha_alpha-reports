namespace Alpha.Reports.Tests.Features.BonusDetail.Criteria
{
    using System;
    using Alpha.Reports.Features.BonusDetail.Criteria;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Alpha.Reports.Models;
    using Xunit;

    [Trait("Category", "BonusDetail")]
    public class GetAgentBonusCriteriaTests
    {
        [Fact]
        public void Constructor_Always_AssignCorrectProperties()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Agent, "member01", new DateTime(2021, 1, 1), new DateTime(2021, 1, 2), DateTime.MinValue, 1, true, 1, 500);

            // Act
            var criteria = new GetAgentBonusCriteria(param);

            // Assert
            Assert.True(
                param.Username == criteria.Username
                && param.FromDate == criteria.FromDate
                && param.ToDate == criteria.ToDate
                && param.StatusId == criteria.StatusId
                && param.IsComplained == criteria.IsComplained
                && param.PageIndex == criteria.PageIndex
                && param.PageSize == criteria.PageSize);
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Agent, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(param);
            var expectedSettingKey = "Age_Rpt_BonusDetail_Agent";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Agent, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsValid_FromDateGreaterThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Agent, null, DateTime.Today, DateTime.Today.AddDays(-1), DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData(-8, false)]
        [InlineData(0, false)]
        [InlineData(8, true)]
        public void IsValid_AgentId_ReturnsExpectedValue(int agentId, bool expected)
        {
            // Arrange
            var param = new BonusDetailParam(agentId, UserLevel.Agent, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
