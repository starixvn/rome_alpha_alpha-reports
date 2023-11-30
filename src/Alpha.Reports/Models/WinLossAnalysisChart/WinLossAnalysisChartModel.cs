namespace Alpha.Reports.Models.WinLossAnalysisChart
{
    public class WinLossAnalysisChartModel
    {
        public WinLossAnalysisChartModel()
        {
        }

        public WinLossAnalysisChartModel(
            string winLossDate,
            decimal turnover,
            decimal winloss)
        {
            WinLossDate = winLossDate;
            Turnover = turnover;
            WinLoss = winloss;
        }

        public string WinLossDate { get; set; }

        public decimal Turnover { get; set; }

        public decimal WinLoss { get; set; }
    }
}