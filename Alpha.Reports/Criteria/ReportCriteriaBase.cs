namespace Alpha.Reports.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class ReportCriteriaBase : CriteriaBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
