namespace Alpha.Reports.Models
{
    public class CustInfo
    {
        public CustInfo(int custId, string custName, byte userLevelId)
        {
            CustId = custId;
            CustName = custName;
            UserLevelId = userLevelId;
        }

        public int CustId { get; }

        public string CustName { get; }

        public byte UserLevelId { get; }
    }
}
