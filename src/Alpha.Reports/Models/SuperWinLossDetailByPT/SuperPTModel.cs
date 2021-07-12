namespace Alpha.Reports.Models.SuperWinLossDetailByPT
{
    public class SuperPTModel
    {
        public SuperPTModel()
        {
        }

        public SuperPTModel(
            int custId,
            decimal superPT,
            int productId,
            string productName)
        {
            CustId = custId;
            SuperPT = superPT;
            ProductId = productId;
            ProductName = productName;
        }

        public int CustId { get; }

        public decimal SuperPT { get; }

        public int ProductId { get; }

        public string ProductName { get; }
    }
}