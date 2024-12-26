namespace Alpha.Reports.Features.WhiteLabelReport.Models
{
    using System;

    public class DailyWinLossModel
    {
        public DateTime WinLostDate { get; set; }

        public string WhiteLabelName { get; set; }

        public string Agent { get; set; }

        public string Master { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal Promotion { get; set; }

        public static DailyWinLossModel operator +(DailyWinLossModel model1, DailyWinLossModel model2)
        {
            model1.MemberWinLoss += model2.MemberWinLoss;
            model1.Promotion += model2.Promotion;

            return model1;
        }
    }
}
