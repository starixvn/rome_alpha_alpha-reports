namespace Alpha.Reports.Tests.Comparers.WinLossDetail
{
    using System.Collections.Generic;
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

        public static bool Equals(List<WinLossDetail> winLossDetailA, List<WinLossDetail> winLossDetailB)
        {
            var isEqual = (winLossDetailA == null && winLossDetailB == null) ||
                (winLossDetailA != null && winLossDetailB != null && winLossDetailA.Count == winLossDetailB.Count);

            var length = winLossDetailA == null ? 0 : winLossDetailA.Count;

            for (var i = 0; i < length; i++)
            {
                if (!Equals(winLossDetailA[i], winLossDetailB[i]))
                {
                    return false;
                }
            }

            return isEqual;
        }
    }
}
