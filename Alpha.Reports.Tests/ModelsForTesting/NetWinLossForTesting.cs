namespace Alpha.Reports.Tests.ModelsForTesting
{
    using System.Text;
    using Reports.Models.WinlossDetails;

    public class NetWinLossForTesting : NetWinLoss
    {
        public NetWinLossForTesting(decimal winloss, decimal comm) : base(winloss, comm)
        {
        }

        public static NetWinLossForTesting FromNetWinLoss(NetWinLoss netWinLoss)
        {
            if (netWinLoss == null)
            {
                return null;
            }

            return new NetWinLossForTesting(netWinLoss.WinLoss, netWinLoss.Comm);
        }

        public override bool Equals(object obj)
        {
            var compareObject = (NetWinLoss)obj;

            var isEqual = WinLoss == compareObject.WinLoss &&
                Comm == compareObject.Comm;

            return isEqual;
        }

        public override int GetHashCode()
        {
            return WinLoss.GetHashCode() ^ Comm.GetHashCode();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("WinLoss: " + WinLoss);
            result.AppendLine("Comm: " + Comm);
            result.AppendLine(base.ToString());

            return result.ToString();
        }
    }
}
