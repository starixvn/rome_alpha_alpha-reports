namespace Alpha.Reports.Services.WinLoss
{
    using System.Collections.Generic;
    using Models.Winloss;
    using Params;

    public interface WinLossService
    {
        IEnumerable<EnteredSportResult> GetEnteredSportResults();

        IEnumerable<WinLossModel> GetWinLoss(WinLossParam param);
    }
}