namespace Alpha.Reports.Services.Impl
{
    public class BaseWinLossDetailService
    {
        private readonly ReportDateService reportDateService;

        public BaseWinLossDetailService(ReportDateService reportDateService)
        {
            this.reportDateService = reportDateService;
        }
    }
}
