namespace Alpha.Reports.DataAccesses.WinLoss
{
    using System.Collections.Generic;
    using Criteria.WinLoss;
    using Models.Winloss;

    public interface WinLossDataAccess
    {
        IEnumerable<WinLossModel> GetWinLoss(WinLossCriteria criteria);
    }
}