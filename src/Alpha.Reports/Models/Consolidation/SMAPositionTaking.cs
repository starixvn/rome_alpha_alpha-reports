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


        public int CustId { get; set; }

        public decimal ValuePT { get; set; }

        public int ProductId { get; set; }
    }
}
