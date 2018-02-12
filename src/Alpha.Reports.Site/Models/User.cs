namespace Alpha.Reports.Site.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Reports.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";

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

        public bool GetHideProduct(int productId) => userSession.GetHideProduct(productId);

        public IList<int> EnableJackpotProductIds => GetEnableProductsForGame(Jackpot);

        public bool IsEnabledJackpot => EnableJackpotProductIds.Any();

        internal virtual IList<int> GetEnableProductsForGame(string gameName)
            => (IList<int>)HttpContext.Current.Session[HttpContext.Current.Session.SessionID + GameEnableSessionKey + gameName];
    }
}