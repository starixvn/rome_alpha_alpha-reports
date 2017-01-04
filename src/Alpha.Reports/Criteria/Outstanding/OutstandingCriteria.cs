using System;
using Fanex.Data.Repository;

namespace Alpha.Reports.Criteria.Outstanding
{
    public class OutstandingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

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
