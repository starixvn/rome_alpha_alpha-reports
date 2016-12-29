using System;

namespace Alpha.Reports.Models.WinLossDetail
{
    public class NetWinLoss
    {
        public NetWinLoss(decimal total)
        {
            Total = total;
        }

        public NetWinLoss(decimal winloss, decimal comm)
        {
            WinLoss = winloss;
            Comm = comm;
            Total = winloss + comm;
        }

        public NetWinLoss(decimal? winloss, decimal? comm, decimal total)
        {
            WinLoss = winloss;
            Comm = comm;
            Total = total;
        }

        public decimal? WinLoss { get; }

        public decimal? Comm { get; }

        public decimal Total { get; }

        public static NetWinLoss Add(NetWinLoss winlossA, NetWinLoss winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return null;
            }

            return new NetWinLoss(
                winloss: winlossA?.WinLoss + winlossB?.WinLoss,
                comm: winlossA?.Comm + winlossB?.Comm,
                total: winlossA.Total + winlossB.Total);
        }
    }
}