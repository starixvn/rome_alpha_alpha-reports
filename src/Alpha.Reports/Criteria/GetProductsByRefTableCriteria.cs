namespace Alpha.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetProductsByRefTableCriteria : CriteriaBase
    {
        public GetProductsByRefTableCriteria(string refTable)
        {
            RefTable = refTable;
        }

        public string RefTable { get; }

        public override string GetSettingKey() => "Age_Rpt_GetProductListByRefTable";

        public override bool IsValid() => true;
    }
}
