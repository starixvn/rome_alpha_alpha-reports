namespace Alpha.Reports.Features.SuperWinLossDetailByPT.Models
{
    public class SuperPtProductModel
    {
        public SuperPtProductModel(
            decimal positionTaking,
            string productName)
        {
            PositionTaking = positionTaking;
            ProductName = productName;
        }

        public decimal PositionTaking { get; }

        public string ProductName { get; }
    }
}