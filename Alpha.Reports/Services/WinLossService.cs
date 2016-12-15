namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Winloss;
    using Params;

    public interface WinLossService
    {
        ReportData<WinLoss> GetWinLosses(WinLossParam param);

        ReportData<EnteredSportResult> GetEnteredSportResults();
    }
}
