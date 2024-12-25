namespace Alpha.Reports.Site.Features.WhiteLabelReport.ViewModels
{
    using System.Collections.Generic;
    using Alpha.Reports.Features.WhiteLabelReport.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using Alpha.Reports.Site.Features.WhiteLabelReport.Requests;
    using Alpha.Reports.Utils;
    using Newtonsoft.Json;

    public class DailyWinLossViewModel : DailyWinLossGetDataViewModel
    {
        public DailyWinLossViewModel(
            DailyWinLossQuery message,
            IEnumerable<IdNameModel> whiteLabels,
            ReportData<DailyWinLossModel> reportData) : base(message.ViewerLevel, reportData)
        {
            DateRangePicker = new DateRangePickerNewViewModel(
                message.FromDate,
                message.ToDate,
                message.MinDate,
                SystemTime.Today,
                message.Language);
            WhiteLabelJson = JsonConvert.SerializeObject(whiteLabels);
            Username = message.Username;
        }

        public DateRangePickerNewViewModel DateRangePicker { get; }

        public string WhiteLabelJson { get; }

        public string Username { get; }
    }
}