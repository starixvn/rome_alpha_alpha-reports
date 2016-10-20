namespace Alpha.Reports.Services.Impl
{
    public class BaseWinLossDetailService
    {
        private readonly ArrangeBetsInDatabaseService arrangeBetDataService;

        public BaseWinLossDetailService(ArrangeBetsInDatabaseService arrangeBetDataService)
        {
            this.arrangeBetDataService = arrangeBetDataService;
        }
    }
}
