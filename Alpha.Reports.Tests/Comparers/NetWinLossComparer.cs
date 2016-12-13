namespace Alpha.Reports.Tests.Comparers
{
    using Reports.Models.WinlossDetails;

    public static class NetWinLossComparer
    {
        public static bool Equals(NetWinLoss netWinLossA, NetWinLoss netWinLossB)
        {
            if (netWinLossA == null && netWinLossB == null)
            {
                return true;
            }

            if (netWinLossA == null || netWinLossB == null)
            {
                return false;
            }

            return netWinLossA.WinLoss == netWinLossB.WinLoss &&
                netWinLossA.Comm == netWinLossB.Comm;
        }
    }
}
