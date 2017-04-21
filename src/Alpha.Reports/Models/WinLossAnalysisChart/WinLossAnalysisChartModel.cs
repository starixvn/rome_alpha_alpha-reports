namespace Alpha.Reports.Models.WinLossAnalysisChart
{
    using Extensions;
    using System;

    public class WinLossAnalysisChartModel
    {
        public WinLossAnalysisChartModel()
        {
        }

        public WinLossAnalysisChartModel(
            DateTime winLossDate,
            decimal turnover,
            decimal winloss)
        {
            WinLossDate = winLossDate;
            Turnover = turnover;
            WinLoss = winloss;
        }

        public DateTime WinLossDate { get; }

        public decimal Turnover { get; }

        public decimal WinLoss { get; }

        public string WinLossDateString => WinLossDate.ToShortMonthAndYear();
    }
}