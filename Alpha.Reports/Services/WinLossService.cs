namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Winloss;
    using Params;

    public interface WinLossService
    {
        ReportData<WinLoss> GetWinLoss(WinLossParam param);

        ReportData<EnteredSportResult> GetEnteredSportResults();
    }
}
