namespace Alpha.Reports.Models.PlayForFun
{
    using Constants;

    public class MemberPlayForFunModel : PlayForFunModel
    {
        public MemberPlayForFunModel()
        {

        }

        public MemberPlayForFunModel(int custId, string custName, bool isDisabled, decimal turnover, decimal winLoss, int betCount)
            : base(custId, custName, turnover, winLoss, betCount)
        {
            IsDisabled = isDisabled;
        }

        public int StatusId => IsDisabled ? StatusIdConstants.Disabled : StatusIdConstants.Enabled;

        public bool IsDisabled { get; }
    }
}
