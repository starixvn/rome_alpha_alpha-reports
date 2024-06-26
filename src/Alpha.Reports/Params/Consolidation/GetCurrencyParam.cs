namespace Alpha.Reports.Params.Consolidation
{
    public class GetCurrencyParam
    {
        public GetCurrencyParam(int custId, string site)
        {
            CustId = custId;
            Site = site;
        }

        public int CustId { get; }

        public string Site { get; }
    }
}
