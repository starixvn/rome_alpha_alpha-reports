namespace Alpha.Reports.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Criteria.WinLossDetail;
    using Models.WinlossDetails;

    public interface WinLossDetailDataAccess
    {
        Tuple<IEnumerable<WinLossDetail>, WinLossDetail, int> GetReport(WinLossDetailCriteria criteria);
    }
}