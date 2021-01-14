namespace Alpha.Reports.Tests.Features.BonusDetail.Models
{
    using System;
    using Alpha.Reports.Features.BonusDetail.Constants;
    using Alpha.Reports.Features.BonusDetail.Models;
    using Xunit;

    [Trait("Category", "BonusDetail")]
    public class BonusEntityTests
    {
        [Fact]
        public void Constructor_Default_AssignsCorrectProperties()
        {
            // Arrange & Act
            var actual = new BonusEntity();

            // Assert
            Assert.True(
                actual.TransId == 0
                && actual.CustId == 0
                && actual.Username == null
                && actual.PaymentDate == null
                && actual.IsComplained == false);
        }

        [Fact]
        public void CreatedDateString_Always_ReturnsCorrectString()
        {
            // Arrange
            var entity = new BonusEntity(new DateTime(2020, 12, 9, 11, 59, 00), 1010, "AA01", "AA0101", 1, "AA0101001", 100, new DateTime(2020, 12, 16), BonusStatusIdConst.Completed, false);

            // Act
            var actual = entity.CreatedDateString;

            // Assert
            Assert.Equal("12/09/2020 11:59 AM", actual);
        }

        [Fact]
        public void PaymentDateString_Null_ReturnsEmptyString()
        {
            // Arrange
            var entity = new BonusEntity(new DateTime(2020, 12, 9, 11, 59, 00), 1010, "AA01", "AA0101", 1, "AA0101001", 100, null, BonusStatusIdConst.Completed, false);

            // Act
            var actual = entity.PaymentDateString;

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void PaymentDateString_HasData_ReturnsCorrectString()
        {
            // Arrange
            var entity = new BonusEntity(new DateTime(2020, 12, 9, 11, 59, 00), 1010, "AA01", "AA0101", 1, "AA0101001", 100, new DateTime(2020, 12, 16), BonusStatusIdConst.Completed, false);

            // Act
            var actual = entity.PaymentDateString;

            // Assert
            Assert.Equal("12/16/2020", actual);
        }

        [Fact]
        public void MinConfirmPaymentDate_Always_ReturnsCorrectString()
        {
            // Arrange
            var entity = new BonusEntity(new DateTime(2020, 12, 9, 11, 59, 00), 1010, "AA01", "AA0101", 1, "AA0101001", 100, new DateTime(2020, 12, 16), BonusStatusIdConst.Completed, false);

            // Act
            var actual = entity.MinConfirmPaymentDate;

            // Assert
            Assert.Equal("12/09/2020", actual);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        public void CanConfirmPaymentDate_Always_ReturnsExpectedValue(byte statusId, bool expected)
        {
            // Arrange
            var entity = new BonusEntity(new DateTime(2020, 12, 9, 11, 59, 00), 1010, "AA01", "AA0101", 1, "AA0101001", 100, new DateTime(2020, 12, 16), statusId, false);

            // Act
            var actual = entity.CanConfirmPaymentDate;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
