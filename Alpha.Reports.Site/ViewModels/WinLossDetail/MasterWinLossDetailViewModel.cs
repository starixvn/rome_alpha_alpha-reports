namespace Alpha.Reports.Site.ViewModels.WinLossDetail
{
    using Models;
    using Models.WinlossDetail;

    public class MasterWinLossDetailViewModel : WinLossDetailViewModel
    {
        public ReportData<MasterWinLossDetail> ReportData { get; set; }
    }
}