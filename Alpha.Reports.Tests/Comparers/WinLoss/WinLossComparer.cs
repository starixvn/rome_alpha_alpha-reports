namespace Alpha.Reports.Tests.Comparers.WinLoss
{
    using System.Collections.Generic;
    using System.Linq;
    using Reports.Models.Winloss;
    using WinLossDetail;

    public static class WinLossComparer
    {
        public static bool Equals(WinLoss winLossA, WinLoss winLossB)
        {
            if (winLossA == null && winLossB == null)
            {
                return true;
            }

            if (winLossA == null || winLossB == null)
            {
                return false;
            }

            return winLossA.CustId == winLossB.CustId &&
                winLossA.CustName == winLossB.CustName &&
                winLossA.GrossComm == winLossB.GrossComm &&
                winLossA.Turnover == winLossB.Turnover &&
                NetWinLossComparer.Equals(winLossA.MemberNetWinLoss, winLossB.MemberNetWinLoss) &&
                NetWinLossComparer.Equals(winLossA.AgentNetWinLoss, winLossB.AgentNetWinLoss) &&
                NetWinLossComparer.Equals(winLossA.MasterNetWinLoss, winLossB.MasterNetWinLoss) &&
                NetWinLossComparer.Equals(winLossA.SuperNetWinLoss, winLossB.SuperNetWinLoss);
        }

        public static bool Equals(IEnumerable<WinLoss> winLossA, IEnumerable<WinLoss> winLossB)
        {
            var listA = winLossA?.ToList();
            var listB = winLossB?.ToList();

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
