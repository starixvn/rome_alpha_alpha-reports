namespace Alpha.Reports.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetProductByCustomerCriteria : CriteriaBase
    {
        public GetProductByCustomerCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_GetProductByCust";

        public override bool IsValid() => CustId > 0;

        public bool IsNotValid() => !IsValid();        
    }
}
