namespace Alpha.Reports.Criteria.MatchWinLossDetail
{
    using Fanex.Data.Repository;

    public class ProductsForMatchWinLossDetailCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_Rpt_MatchWinLossDetail_GetProductList";

        public override bool IsValid() => true;
    }
}
