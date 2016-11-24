namespace Alpha.Reports.Tests.ModelsForTesting
{
    using System.Text;
    using Reports.Models.WinlossDetails;

    public class WinLossDetailForTesting : WinLossDetail
    {
        public WinLossDetailForTesting(
            int custId,
            string custName,
            decimal grossComm,
            decimal netTurnover,
            decimal turnover,
            int numberOfTickets,
            NetWinLoss memberNetWinLoss,
            NetWinLoss agentNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss superNetWinLoss) : base(
                custId,
                custName,
                grossComm,
                netTurnover,
                turnover,
                numberOfTickets,
                memberNetWinLoss,
                agentNetWinLoss,
                masterNetWinLoss,
                superNetWinLoss)
        {
        }

        public static WinLossDetailForTesting FromWinLossDetail(WinLossDetail winLossDetail)
        {
            if (winLossDetail == null)
            {
                return null;
            }

            return new WinLossDetailForTesting(
                winLossDetail.CustId,
                winLossDetail.CustName,
                winLossDetail.GrossComm,
                winLossDetail.NetTurnover,
                winLossDetail.Turnover,
                winLossDetail.NumberOfTickets,
                NetWinLossForTesting.FromNetWinLoss(winLossDetail.MemberNetWinLoss),
                NetWinLossForTesting.FromNetWinLoss(winLossDetail.AgentNetWinLoss),
                NetWinLossForTesting.FromNetWinLoss(winLossDetail.MasterNetWinLoss),
                NetWinLossForTesting.FromNetWinLoss(winLossDetail.SuperNetWinLoss));
        }

        public override bool Equals(object obj)
        {
            var compareObject = (WinLossDetailForTesting)obj;
            var isEqual =
                CustId == compareObject.CustId &&
                CustName == compareObject.CustName &&
                GrossComm == compareObject.GrossComm &&
                NetTurnover == compareObject.NetTurnover &&
                Turnover == compareObject.Turnover &&
                NumberOfTickets == compareObject.NumberOfTickets &&
                Equals(MemberNetWinLoss, compareObject.MemberNetWinLoss) &&
                Equals(AgentNetWinLoss, compareObject.AgentNetWinLoss) &&
                Equals(MasterNetWinLoss, compareObject.MasterNetWinLoss) &&
                Equals(SuperNetWinLoss, compareObject.SuperNetWinLoss);

            return isEqual;
        }

        public override int GetHashCode()
        {
            return CustId.GetHashCode() ^
                CustName.GetHashCode() ^
                GrossComm.GetHashCode() ^
                NetTurnover.GetHashCode() ^
                Turnover.GetHashCode() ^
                NumberOfTickets.GetHashCode() ^
                MemberNetWinLoss.GetHashCode() ^
                AgentNetWinLoss.GetHashCode() ^
                MasterNetWinLoss.GetHashCode() ^
                SuperNetWinLoss.GetHashCode();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("CustId: " + CustId);
            result.AppendLine("CustName: " + CustName);
            result.AppendLine("GrossComm: " + GrossComm);
            result.AppendLine("NetTurnover: " + NetTurnover);
            result.AppendLine("Turnover: " + Turnover);
            result.AppendLine("NumberOfTickets: " + NumberOfTickets);
            result.AppendLine("MemberNetWinLoss: " + MemberNetWinLoss);
            result.AppendLine("AgentNetWinLoss: " + AgentNetWinLoss);
            result.AppendLine("MasterNetWinLoss: " + MasterNetWinLoss);
            result.AppendLine("SuperNetWinLoss: " + SuperNetWinLoss);
            result.AppendLine(base.ToString());

            return result.ToString();
        }
    }
}
