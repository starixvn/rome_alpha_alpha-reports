namespace Alpha.Reports.Models.PlayForFun
{
    public class PlayForFunModel
    {
        public PlayForFunModel()
        {

        }

        public PlayForFunModel(int custId, string custName, decimal turnover, decimal winLoss, int betCount)
        {
            CustId = custId;
            CustName = custName;
            Turnover = turnover;
            WinLoss = winLoss;
            BetCount = betCount;
        }

        public int CustId { get; protected set; }

        public string CustName { get; protected set; }

        public decimal Turnover { get; protected set; }

        public decimal WinLoss { get; protected set; }

        public int BetCount { get; protected set; }

        public virtual void Add(PlayForFunModel playForFunModel)
        {
            Turnover += playForFunModel.Turnover;
            WinLoss += playForFunModel.WinLoss;
            BetCount += playForFunModel.BetCount;
        }
    }
}
