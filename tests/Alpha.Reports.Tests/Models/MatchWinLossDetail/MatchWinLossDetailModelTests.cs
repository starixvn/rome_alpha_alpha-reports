namespace Alpha.Reports.Tests.Models.MatchWinLossDetail
{
    using Extensions;
    using Fixtures;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossDetailModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MatchWinLossDetailModelTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void CompanyNetWinLoss_NullMemberNetWinLoss_ReturnsCorrectValue()
        {
            // Arrange
            var model = new MatchWinLossDetailModel(
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                null,
                new NetWinLoss(1, 2, 3),
                new NetWinLoss(4, 5, 6),
                new NetWinLoss(7, 8, 9));
            var expectedCompanyNetWinLoss = new NetWinLoss(0, 15, -27);

            // Act
            var actualCompanyNetWinLoss = model.CompanyNetWinLoss;

            // Assert
            AssertExtension.Equal(expectedCompanyNetWinLoss, actualCompanyNetWinLoss);
        }

        [Fact]
        public void CompanyNetWinLoss_NullMemberAgentNetWinLoss_ReturnsCorrectValue()
        {
            // Arrange
            var model = new MatchWinLossDetailModel(
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                null,
                null,
                new NetWinLoss(4, 5, 6),
                new NetWinLoss(7, 8, 9));
            var expectedCompanyNetWinLoss = new NetWinLoss(0, 13, -24);

            // Act
            var actualCompanyNetWinLoss = model.CompanyNetWinLoss;

            // Assert
            AssertExtension.Equal(expectedCompanyNetWinLoss, actualCompanyNetWinLoss);
        }

        [Fact]
        public void CompanyNetWinLoss_NullMemberAgentMasterNetWinLoss_ReturnsCorrectValue()
        {
            // Arrange
            var model = new MatchWinLossDetailModel(
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                null,
                null,
                null,
                new NetWinLoss(7, 8, 9));
            var expectedCompanyNetWinLoss = new NetWinLoss(0, 8, -15);

            // Act
            var actualCompanyNetWinLoss = model.CompanyNetWinLoss;

            // Assert
            AssertExtension.Equal(expectedCompanyNetWinLoss, actualCompanyNetWinLoss);
        }

        [Fact]
        public void CompanyNetWinLoss_NullNetWinLoss_ReturnsCorrectValue()
        {
            // Arrange
            var model = new MatchWinLossDetailModel(
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                null,
                null,
                null,
                null);
            var expectedCompanyNetWinLoss = new NetWinLoss(0, 0, 0);

            // Act
            var actualCompanyNetWinLoss = model.CompanyNetWinLoss;

            // Assert
            AssertExtension.Equal(expectedCompanyNetWinLoss, actualCompanyNetWinLoss);
        }
    }
}
