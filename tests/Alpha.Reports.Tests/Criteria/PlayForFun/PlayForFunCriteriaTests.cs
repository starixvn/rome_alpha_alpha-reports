namespace Alpha.Reports.Tests.Criteria.PlayForFun
{
    using System;
    using Constants;
    using Fixtures;
    using Reports.Criteria.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("AnyValueCreatorCollection")]
    public class PlayForFunCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public PlayForFunCriteriaTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var criteria = anyValueCreatorFixture.Create<PlayForFunCriteria>();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => criteria.GetSettingKey());
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnFalse(int custId)
        {
            // Arrange
            var criteria = new PlayForFunCriteria(
                custId,
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ToDateLessThanFromDate_ReturnFalse()
        {
            // Arrange
            var toDate = DefaultVariables.CurrentDate;
            var fromDate = toDate.AddDays(1);
            var criteria = new PlayForFunCriteria(
                1,
                fromDate,
                toDate,
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = new PlayForFunCriteria(
                1,
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
