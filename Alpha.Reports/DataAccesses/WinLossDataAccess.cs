namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.WinLoss;
    using Models.Winloss;

    public interface WinLossDataAccess
    {
        List<WinLoss> GetWinLosses(WinLossCriteria criteria);

        List<EnteredSportDaily> GetEnteredSportDailies();
    }
}