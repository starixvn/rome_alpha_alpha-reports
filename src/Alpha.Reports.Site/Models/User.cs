namespace Alpha.Reports.Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Configurations;
    using Constants;
    using Reports.Constants;
    using Reports.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";

        private readonly IUserSession userSession;

        public User(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Site => userSession.Site;

        public string Language => userSession.Language;

        public byte LevelId => userSession.RoleId;

        public string LanguageKey => userSession.LangKey;

        public string CurrencyName => userSession.Currency;

        public int CurrencyId => userSession.CurrencyId;

        public bool IsShowConsolidatedReport => userSession.IsShowConsolidatedReport;

        public bool GetHideProduct(int productId) => userSession.GetHideProduct(productId);

        public IList<int> EnableJackpotProductIds => GetEnableProductsForGame(Jackpot);

        public bool IsEnabledJackpot
            => EnableJackpotProductIds.Count == 1 && EnableJackpotProductIds.Contains(ProductIdConstants.RNGCasino)
                ? userSession.IsCasinoSync
                : EnableJackpotProductIds.Any();

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>(SessionKey.CustomerDatelimit);

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public bool IsEnableSabaPromotion 
            => SiteSettings.OfferSabaPromotionSites.Any(site => site == "*" || site.Equals(userSession.Site, StringComparison.OrdinalIgnoreCase));

        internal virtual IList<int> GetEnableProductsForGame(string gameName)
            => userSession.GetProperty<IList<int>>(SessionKey.GameEnable + gameName) ?? new List<int>();
    }
}
