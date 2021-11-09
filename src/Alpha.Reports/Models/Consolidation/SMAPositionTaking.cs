namespace Alpha.Reports.Models.Consolidation
{
    public class SMAPositionTaking
    {
        public SMAPositionTaking()
        {
        }

        public SMAPositionTaking(int custId, decimal valuePT, int productId)
        {
            CustId = custId;
            ValuePT = valuePT;
            ProductId = productId;
        }


        public int CustId { get; }

        public decimal ValuePT { get; }

        public int ProductId { get; }
    }
}
