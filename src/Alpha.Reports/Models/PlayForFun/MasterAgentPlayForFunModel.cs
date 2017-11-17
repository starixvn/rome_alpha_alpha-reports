namespace Alpha.Reports.Models.PlayForFun
{
    public class MasterAgentPlayForFunModel : PlayForFunModel
    {
        public MasterAgentPlayForFunModel()
        {

        }

        public MasterAgentPlayForFunModel(int custId, string custName, int numberOfPlayers, decimal turnover, decimal winLoss, int betCount)
            : base(custId, custName, turnover, winLoss, betCount)
        {
            NumberOfPlayers = numberOfPlayers;
        }

        public int NumberOfPlayers { get; private set; }

        public override void Add(PlayForFunModel playForFunModel)
        {
            base.Add(playForFunModel);
            NumberOfPlayers += ((MasterAgentPlayForFunModel)playForFunModel).NumberOfPlayers;
        }
    }
}
