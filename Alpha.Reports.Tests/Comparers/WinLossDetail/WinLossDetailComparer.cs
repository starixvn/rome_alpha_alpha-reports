namespace Alpha.Reports.Tests.Comparers.WinLossDetail
{
    using System.Collections.Generic;
    using System.Linq;
    using Reports.Models.WinlossDetails;

    public static class WinLossDetailComparer
    {
        public static bool Equals(WinLossDetail winLossDetailA, WinLossDetail winLossDetailB)
        {
            if (winLossDetailA == null && winLossDetailB == null)
            {
                return true;
            }

            if (winLossDetailA == null || winLossDetailB == null)
            {
                return false;
            }

            return winLossDetailA.CustId == winLossDetailB.CustId &&
                winLossDetailA.CustName == winLossDetailB.CustName &&
                winLossDetailA.GrossComm == winLossDetailB.GrossComm &&
                winLossDetailA.NetTurnover == winLossDetailB.NetTurnover &&
                winLossDetailA.Turnover == winLossDetailB.Turnover &&
                winLossDetailA.NumberOfTickets == winLossDetailB.NumberOfTickets &&
                NetWinLossComparer.Equals(winLossDetailA.MemberNetWinLoss, winLossDetailB.MemberNetWinLoss) &&
                NetWinLossComparer.Equals(winLossDetailA.AgentNetWinLoss, winLossDetailB.AgentNetWinLoss) &&
                NetWinLossComparer.Equals(winLossDetailA.MasterNetWinLoss, winLossDetailB.MasterNetWinLoss) &&
                NetWinLossComparer.Equals(winLossDetailA.SuperNetWinLoss, winLossDetailB.SuperNetWinLoss);
        }

        public static bool Equals(IEnumerable<WinLossDetail> winLossDetailA, IEnumerable<WinLossDetail> winLossDetailB)
        {
            var listA = winLossDetailA?.ToList();
            var listB = winLossDetailB?.ToList();

            var isEqual = (listA == null && listB == null) ||
                (listA != null && listB != null && listA.Count == listB.Count);

            var length = listA == null ? 0 : listA.Count;

            for (var i = 0; i < length; i++)
            {
                if (!Equals(listA[i], listB[i]))
                {
                    return false;
                }
            }

            return isEqual;
        }
    }
}
