namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using Models.Winloss;
    using Params;

    public interface WinLossService
    {
        IEnumerable<EnteredMatchResult> GetEnteredSportResults();

        IEnumerable<WinLoss> GetWinLoss(WinLossParam param); 
    }
}
