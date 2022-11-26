namespace Alpha.Reports.Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Configurations;
    using Constants;
    using Reports.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string MainDomainSessionKey = "MainDomain";
        private const string AutoFollowBetIsDummyAgentSessionKey = "AutoFollowBetIsDummyAgent";

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

        public IList<int> EnableJackpotProductIds => GetEnableProductsForGame(Jackpot);

        public bool IsEnabledJackpot => EnableJackpotProductIds.Any();

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>(SessionKey.CustomerDatelimit);

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public bool IsEnableSabaPromotion
            => SiteSettings.OfferSabaPromotionSites.Any(site => site == "*" || site.Equals(userSession.Site, StringComparison.OrdinalIgnoreCase));

        public bool IsEnableSabaPromotionForHeadline
            => SiteSettings.OfferSabaPromotionHeadlineSites.Any(site => site == "*" || site.Equals(userSession.Site, StringComparison.OrdinalIgnoreCase))
            || IsEnableSabaPromotion;

        public int SubAccId => userSession.SubAccId;

        public string IP => userSession.UserLoginIp;

        public bool IsInternal => userSession.IsInternal;

        public bool IsProtectedMode => userSession.IsProtectedMode;
        
        public string DarkModeName => userSession.DarkModeName;

        public string MainDomain => userSession?.GetProperty<string>(MainDomainSessionKey);

        public bool AutoFollowBetIsDummyAgent => userSession.GetProperty<bool>(AutoFollowBetIsDummyAgentSessionKey);

        internal virtual IList<int> GetEnableProductsForGame(string gameName)
            => userSession.GetProperty<IList<int>>(SessionKey.GameEnable + gameName) ?? new List<int>();

        public string MainSessionID => userSession?.GetProperty<string>(SessionKey.MainSessionID);

        public string SessionID => userSession?.NetSessionId;

        public int OnUserId => userSession.OnUserId;
    }
}
