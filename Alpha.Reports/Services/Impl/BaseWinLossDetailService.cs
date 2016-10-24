namespace Alpha.Reports.Services.Impl
{
    public class BaseWinLossDetailService
    {
        private readonly ReportDateService arrangeBetDataService;

        public BaseWinLossDetailService(ReportDateService arrangeBetDataService)
        {
            this.arrangeBetDataService = arrangeBetDataService;
        }
    }
}
