namespace Alpha.Reports.Tests.TestCases
{
    using System.Collections;
    using System.Collections.Generic;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Reports.Models.WinlossDetails;

    public class WinLossTestCases : IEnumerable<object[]>
    {
        public List<WinLoss> superWinLosses = new List<WinLoss>();
        public WinLoss expectedSuperGrantTotal;

        public List<WinLoss> masterWinLosses = new List<WinLoss>();
        public WinLoss expectedMasterGrantTotal;

        public List<WinLoss> agentWinLosses = new List<WinLoss>();
        public WinLoss expectedAgentGrantTotal;

        public WinLossTestCases()
        {
            BuildSuperDataTest();
            BuildMasterDataTest();
            BuildAgentDataTest();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { superWinLosses, UserLevel.Super, expectedSuperGrantTotal };
            yield return new object[] { masterWinLosses, UserLevel.Master, expectedMasterGrantTotal };
            yield return new object[] { agentWinLosses, UserLevel.Agent, expectedAgentGrantTotal };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void BuildSuperDataTest()
        {
            superWinLosses.Add(new WinLoss(
                 custId: 1382145,
                 custName: "ACGUUS38",
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69)));

            superWinLosses.Add(new WinLoss(
                 custId: 18187823,
                 custName: "ACGUUSOP",
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: null,
                 masterNetWinLoss: new NetWinLoss((decimal)9.65),
                 superNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15)));

            expectedSuperGrantTotal = new WinLoss(
               custId: 0,
               custName: "Total",
               turnover: (decimal)375732807.36,
               grossComm: (decimal)2998559.76,
               memberNetWinLoss: null,
               agentNetWinLoss: null,
               masterNetWinLoss: new NetWinLoss((decimal)-111173383.51),
               superNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54));
        }

        private void BuildMasterDataTest()
        {
            masterWinLosses.Add(new WinLoss(
                 custId: 1382145,
                 custName: "ACGUUS3801",
                 turnover: (decimal)375732407.36,
                 grossComm: (decimal)2998557.31,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)-111173393.16),
                 masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.69),
                 superNetWinLoss: null));

            masterWinLosses.Add(new WinLoss(
                 custId: 18187823,
                 custName: "ACGUUS3802",
                 turnover: (decimal)400.00,
                 grossComm: (decimal)2.45,
                 memberNetWinLoss: null,
                 agentNetWinLoss: new NetWinLoss((decimal)9.65),
                 masterNetWinLoss: new NetWinLoss((decimal)0.00, (decimal)0.15),
                 superNetWinLoss: null));

            expectedMasterGrantTotal = new WinLoss(
                  custId: 0,
                  custName: "Total",
                  turnover: (decimal)375732807.36,
                  grossComm: (decimal)2998559.76,
                  memberNetWinLoss: null,
                  agentNetWinLoss: new NetWinLoss((decimal)-111173383.51),
                  masterNetWinLoss: new NetWinLoss((decimal)74561618.35, (decimal)-374728.54),
                  superNetWinLoss: null);
        }

        private void BuildAgentDataTest()
        {
            agentWinLosses.Add(new WinLoss(
                 custId: 1382145,
                 custName: "ACGUUS3838000",
                 turnover: (decimal)4000.00,
                 grossComm: (decimal)21.00,
                 memberNetWinLoss: new NetWinLoss((decimal)-2025.00, (decimal)21.00),
                 agentNetWinLoss: new NetWinLoss((decimal)992.25, (decimal)-10.29),
                 masterNetWinLoss: null,
                 superNetWinLoss: null));

            agentWinLosses.Add(new WinLoss(
                 custId: 18187823,
                 custName: "ACGUUS3838006",
                 turnover: (decimal)2225.00,
                 grossComm: (decimal)13.83,
                 memberNetWinLoss: new NetWinLoss((decimal)-551.25, (decimal)0.00),
                 agentNetWinLoss: new NetWinLoss((decimal)275.63, (decimal)6.91),
                 masterNetWinLoss: null,
                 superNetWinLoss: null));

            expectedAgentGrantTotal = new WinLoss(
                  custId: 0,
                  custName: "Total",
                  turnover: (decimal)6225.00,
                  grossComm: (decimal)34.83,
                  memberNetWinLoss: new NetWinLoss((decimal)-2576.25, (decimal)21.00),
                  agentNetWinLoss: new NetWinLoss((decimal)1267.88, (decimal)-3.38),
                  masterNetWinLoss: null,
                  superNetWinLoss: null);
        }
    }
}
