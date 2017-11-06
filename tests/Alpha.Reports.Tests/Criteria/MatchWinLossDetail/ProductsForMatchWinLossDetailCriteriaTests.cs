namespace Alpha.Reports.Tests.Criteria.MatchWinLossDetail
{
    using Reports.Criteria.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("AnyValueCreatorCollection")]
    public class ProductsForMatchWinLossDetailCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnCorrectSettingKey()
        {
            //// Arrange
            var expectedSettingKey = "Age_Rpt_MatchWinLossDetail_GetProductList";
            var criteria = new ProductsForMatchWinLossDetailCriteria();

            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedSettingKey, act);
        }

        [Fact]
        public void IsValid_Always_ReturnTrue()
        {
            //// Arrange            
            var criteria = new ProductsForMatchWinLossDetailCriteria();

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }
    }
}
