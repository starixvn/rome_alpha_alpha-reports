namespace Alpha.Reports.Models
{
    public class CustInfo
    {
        public CustInfo(int custId, string custName, byte custLevelId)
        {
            CustId = custId;
            CustName = custName;
            CustLevelId = custLevelId;
        }

        public int CustId { get; }

        public string CustName { get; }

        public byte CustLevelId { get; }
    }
}