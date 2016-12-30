namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLoss;

    public class EnteredSportResultQuery : IRequest<EnteredResultViewModel>
    {
        public EnteredSportResultQuery(User user)
        {
            CustId = user.CustId;
            UserLevelId = user.UserLevelId;

            Language = user.Language;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte UserLevelId { get; set; }

        public string Language { get; }
    }
}