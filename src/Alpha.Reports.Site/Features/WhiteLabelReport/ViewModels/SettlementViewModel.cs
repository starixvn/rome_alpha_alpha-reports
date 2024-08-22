namespace Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels
{
    using System.Collections.Generic;
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.WhiteLabelReport.Requests;
    using Alpha.Reports.Site.ViewModels.Components;
    using Newtonsoft.Json;

    public class SettlementViewModel : SettlementGetDataViewModel
    {
        public SettlementViewModel(
            SettlementQuery message,
            IEnumerable<IdNameModel> whiteLabels,
            ReportData<SettlementModel> reportData) : base(message.ViewerLevel, reportData)
        {
            MonthPickerViewModel = new MonthPickerViewModel(message.MinDate, message.FromDate, message.ToDate);
            WhiteLabelJson = JsonConvert.SerializeObject(whiteLabels);
            Username = message.Username;
        }

        public MonthPickerViewModel MonthPickerViewModel { get; }

        public string WhiteLabelJson { get; }

        public string Username { get; }
    }
}