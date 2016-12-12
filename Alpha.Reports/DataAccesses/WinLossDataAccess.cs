namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.WinLoss;
    using Models;
    using Models.Winloss;

    public interface WinLossDataAccess
    {
        ReportData<WinLoss> GetWinLoss(WinLossCriteria criteria);

        List<EnteredSportDaily> GetEnteredSportDailies();
    }
}