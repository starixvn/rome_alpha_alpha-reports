namespace Alpha.Reports.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.WinLoss;
    using Models.Winloss;

    public interface WinLossDataAccess
    {
        IEnumerable<WinLoss> GetWinLoss(WinLossCriteria criteria);
    }
}