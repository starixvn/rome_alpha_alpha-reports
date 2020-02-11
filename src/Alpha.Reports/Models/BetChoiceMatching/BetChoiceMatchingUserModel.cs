namespace Alpha.Reports.Models.BetChoiceMatching
{
    using System.Collections.Generic;

    public class BetChoiceMatchingUserModel
    {
        public BetChoiceMatchingUserModel(BetChoiceMatchingModel item)
        {
            CustId = item.CustId;
            Username = item.Username;
            Stake = item.Stake;
            MemberWinLoss = item.MemberWinLoss;
            SuperWinLoss = item.SuperWinLoss;
            Company = item.Company;
            IsRunning = item.IsRunning;
            Items = new List<BetChoiceMatchingModel> { item };
        }

        public int CustId { get; }

        public string Username { get; }

        public decimal Stake { get; private set; }

        public decimal MemberWinLoss { get; private set; }

        public decimal SuperWinLoss { get; private set; }

        public decimal Company { get; private set; }

        public bool IsRunning { get; }

        public List<BetChoiceMatchingModel> Items { get; }

        public void Add(BetChoiceMatchingModel item)
        {
            Items.Add(item);
            Stake += item.Stake;
            MemberWinLoss += item.MemberWinLoss;
            SuperWinLoss += item.SuperWinLoss;
            Company += item.Company;
        }
    }
}
