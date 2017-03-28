using System;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Tests.Fixtures;
using Xunit;

namespace Alpha.Reports.Tests.Models.Statement
{
    [Trait("Category", "Statement")]
    [Collection("AnyValueCreatorCollection")]
    public class StatementDetailModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public StatementDetailModelTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Theory]
        [InlineData(null, 10)]
        [InlineData(0.00, 10)]
        [InlineData(10.00, 20)]
        [InlineData(20.00, 30)]
        public void Add_Always_AddsAmountValue(double? initAmount, decimal expectedValue)
        {
            // Arrange
            var model = new StatementDetailModel(
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>(),
                (decimal?)initAmount,
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>());

            var addedValue = new StatementDetailModel(
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>(),
                10,
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>());

            // Act
            model.Add(addedValue);

            // Assert
            Assert.Equal(expectedValue, model.Amount);
        }

        [Fact]
        public void FullTransDateName_Always_ReturnsCorrectName()
        {
            // Arrange
            var model = new StatementDetailModel(
                new DateTime(2017, 4, 10),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<decimal>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>());
            var expectedFullTransDateName = "Monday";

            // Act
            var actualFullTransDateName = model.FullTransDateName;

            // Assert
            Assert.Equal(expectedFullTransDateName, actualFullTransDateName);
        }

        [Fact]
        public void TransDateString_Always_ReturnsTransDateString()
        {
            // Arrange
            var model = new StatementDetailModel(
                new DateTime(2017, 4, 10, 1, 2, 3),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<decimal>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>());
            var expectedTransDateString = "04/10/2017";

            // Act
            var actualTransDateString = model.TransDateString;

            // Assert
            Assert.Equal(expectedTransDateString, actualTransDateString);
        }

        [Fact]
        public void TransTimeString_Always_ReturnsTransTimeString()
        {
            // Arrange
            var model = new StatementDetailModel(
                new DateTime(2017, 4, 10, 1, 2, 3),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<decimal>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<string>());
            var expectedTransTimeString = "01:02:03 AM";

            // Act
            var actualTransTimeString = model.TransTimeString;

            // Assert
            Assert.Equal(expectedTransTimeString, actualTransTimeString);
        }

        [Fact]
        public void CreateDefault_Always_ReturnsDefaultModel()
        {
            // Act
            var model = StatementDetailModel.CreateDefault();

            // Assert
            Assert.True(model != null && model.Amount == null);
        }
    }
}
