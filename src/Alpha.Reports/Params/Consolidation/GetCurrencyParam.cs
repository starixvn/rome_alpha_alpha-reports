namespace Alpha.Reports.Params.Consolidation
{
    public class GetCurrencyParam
    {
        public GetCurrencyParam(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}
