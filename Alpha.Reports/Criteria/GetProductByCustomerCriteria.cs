namespace Alpha.Reports.Criteria
{
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

        public override bool Equals(object obj)
        {
            if (obj?.GetType() != GetType())
            {
                return false;
            }

            return CustId == (obj as GetProductByCustomerCriteria).CustId;
        }

        public override int GetHashCode()
        {
            return  new { CustId }.GetHashCode();
        }
    }
}
