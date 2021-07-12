namespace Alpha.Reports.Models.SuperWinLossDetailByPT
{
    using System.Collections.Generic;

    public class SuperWinLossDetailByPTModel
    {
        public SuperWinLossDetailByPTModel(
            decimal positionTaking,
            IEnumerable<SuperWinLossDetailModel> superWinLossDetails)
        {
            PositionTaking = positionTaking;
            SuperWinLossDetails = superWinLossDetails;
        }

        public decimal PositionTaking { get; }

        public string PositionTakingText
        {
            get
            {
                const decimal percentage = 100.0m;
                decimal positionTakingPercentage = PositionTaking * percentage;
                return string.Concat(positionTakingPercentage.ToString("##0"), "% {0}");
            }
        }

        public IEnumerable<SuperWinLossDetailModel> SuperWinLossDetails { get; }

        public SuperWinLossDetailModel Total
        {
            get
            {
                var total = new SuperWinLossDetailModel();
                foreach (var winLossDetail in SuperWinLossDetails)
                {
                    total.Add(winLossDetail);
                }

                return total;
            }
        }
    }
}