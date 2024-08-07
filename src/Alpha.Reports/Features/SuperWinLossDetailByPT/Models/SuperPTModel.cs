namespace Alpha.Reports.Features.SuperWinLossDetailByPT.Models
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

        public int CustId { get; set; }

        public decimal SuperPT { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }
}