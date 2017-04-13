namespace Alpha.Reports.Site.ViewModels.PlayForFun
{
    using Reports.Models.PlayForFun;

    public class MemberPlayForFunViewModel : MemberPlayForFunModel
    {
        public const string SortByStatusName = nameof(StatusName);

        public MemberPlayForFunViewModel(int custId, string custName, decimal turnover, decimal memberWinLoss, int playCount, bool statusId, string statusName)
            : base(custId, custName, statusId, turnover, memberWinLoss, playCount)
        {
            StatusName = statusName;
        }

        public string StatusName { get; }
    }
}