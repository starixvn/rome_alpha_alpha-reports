namespace Alpha.Reports.Params
{
    public class ProductByCustomerParam
    {
        public ProductByCustomerParam(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}