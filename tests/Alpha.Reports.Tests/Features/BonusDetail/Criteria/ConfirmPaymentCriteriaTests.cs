namespace Alpha.Reports.Tests.Features.BonusDetail.Criteria
{
    using System;
    using Alpha.Reports.Features.BonusDetail.Criteria;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Xunit;

    [Trait("Category", "BonusDetail")]
    public class ConfirmPaymentCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new ConfirmPaymentParam(8, 0, 123, 1001, DateTime.Today, "::1", "E");
            var criteria = new ConfirmPaymentCriteria(
                param.CustId, param.SubAccId, param.TransId, param.PaymentDate, param.ViewerId, param.LoginIP, param.Language);
            var expectedSettingKey = "Age_Bonus_ConfirmPayment";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-123, false)]
        [InlineData(0, false)]
        [InlineData(123, true)]
        public void IsValid_CustId_ReturnsExpectedValue(int custId, bool expected)
        {
            // Arrange
            var param = new ConfirmPaymentParam(8, 0, custId, 1001, DateTime.Today, "::1", "E");
            var criteria = new ConfirmPaymentCriteria(
                param.CustId, param.SubAccId, param.TransId, param.PaymentDate, param.ViewerId, param.LoginIP, param.Language);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1001, false)]
        [InlineData(0, false)]
        [InlineData(1001, true)]
        public void IsValid_TransId_ReturnsExpectedValue(long transId, bool expected)
        {
            // Arrange
            var param = new ConfirmPaymentParam(8, 0, 123, transId, DateTime.Today, "::1", "E");
            var criteria = new ConfirmPaymentCriteria(
                param.CustId, param.SubAccId, param.TransId, param.PaymentDate, param.ViewerId, param.LoginIP, param.Language);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-8, false)]
        [InlineData(0, false)]
        [InlineData(8, true)]
        public void IsValid_UserId_ReturnsExpectedValue(int userId, bool expected)
        {
            // Arrange
            var param = new ConfirmPaymentParam(userId, 0, 123, 1001, DateTime.Today, "::1", "E");
            var criteria = new ConfirmPaymentCriteria(
                param.CustId, param.SubAccId, param.TransId, param.PaymentDate, param.ViewerId, param.LoginIP, param.Language);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
