namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Models;

    public class PokerGamesWinLossDetailParam : WinLossDetailParam
    {
        public PokerGamesWinLossDetailParam(
            int custId,
            int pageIndex,
            int pageSize,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            UserLevel reportLevel,
            string sortByPropertyName,
            bool isDescendingSort,
            bool isHistoricReport) :
            base(custId, pageIndex, pageSize, fromDate, toDate, limitedDate, new List<int> { ProductIdConstants.PokerGames }, reportLevel, sortByPropertyName, isDescendingSort, isHistoricReport, Enumerable.Empty<int>())
        {
        }
    }
}