namespace Alpha.Reports.Tests.Criteria.BetChoiceMatching
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceUplinesCriteriaTests
    {
        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var criteria = new BetChoiceUplinesCriteria(1);

            // Assert
            Assert.True(criteria.SearchAccountId == 1
                && criteria.GetSettingKey() == "Age_Customer_Upline_Sel"
                && criteria.IsValid());
        }
    }
}
