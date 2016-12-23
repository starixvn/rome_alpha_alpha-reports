namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Models.Winloss;
    using Params;

    public interface WinLossService
    {
        IEnumerable<WinLoss> GetWinLoss(WinLossParam param);

        IEnumerable<EnteredSportResult> GetEnteredSportResults();
    }
}
