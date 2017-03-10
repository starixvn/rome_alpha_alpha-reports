namespace Alpha.Reports.Models.PlayForFun
{
    public class PlayForFunModel
    {
        public PlayForFunModel(int custId, string custName, decimal turnover, decimal winLoss, int betCount)
        {
            CustId = custId;
            CustName = custName;
            Turnover = turnover;
            WinLoss = winLoss;
            BetCount = betCount;
        }

        public int CustId { get; }

        public string CustName { get; }

        public decimal Turnover { get; private set; }

        public decimal WinLoss { get; private set; }

        public int BetCount { get; private set; }

        public virtual void Add(PlayForFunModel playForFunModel)
        {
            Turnover += playForFunModel.Turnover;
            WinLoss += playForFunModel.WinLoss;
            BetCount += playForFunModel.BetCount;
        }
    }
}
