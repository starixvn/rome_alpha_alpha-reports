namespace Alpha.Reports.Params
{
    public class GetProductByCustomerParam
    {
        public GetProductByCustomerParam(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}
