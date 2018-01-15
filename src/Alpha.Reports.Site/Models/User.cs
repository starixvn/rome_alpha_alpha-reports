namespace Alpha.Reports.Site.Models
{
    using Constants;
    using Reports.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private readonly IUserSession userSession;

        public User(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Language => userSession.Language;

        public byte LevelId => userSession.RoleId;

        public string LanguageKey => userSession.LangKey;

        public string CurrencyName => userSession.Currency;

        public bool IsShowConsolidatedReport => userSession.IsShowConsolidatedReport;

        public bool IsEnableJackpot => !userSession.Currency.Equals(CurrencyConstant.HKK);

        public bool GetHideProduct(int productId) => userSession.GetHideProduct(productId);
    }
}