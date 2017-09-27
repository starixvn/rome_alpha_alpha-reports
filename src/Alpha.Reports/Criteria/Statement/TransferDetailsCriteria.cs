namespace Alpha.Reports.Criteria.Statement
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class TransferDetailsCriteria : CriteriaBase
    {
        public TransferDetailsCriteria(
            int custId,
            DateTime transferDate,
            string storeMappingSuffix)
        {
            CustId = custId;
            TransferDate = transferDate;
            StoreMappingSuffix = storeMappingSuffix;
        }

        [SpParam(SqlParamName = "WinLostDate")]
        public DateTime TransferDate { get; }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public string StoreMappingSuffix { get; }

        public override string GetSettingKey() => "Statement_Report_TransferDetails" + StoreMappingSuffix;

        public override bool IsValid() => CustId > 0;
    }
}
