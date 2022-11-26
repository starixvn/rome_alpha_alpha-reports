namespace Alpha.Reports.Models
{
    using System;
    using System.Collections.Generic;

    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string Site { get; }

        byte LevelId { get; }

        string Language { get; }

        string LanguageKey { get; }

        string CurrencyName { get; }

        int CurrencyId { get; }

        bool IsShowConsolidatedReport { get; }

        bool IsEnabledJackpot { get; }

        IList<int> EnableJackpotProductIds { get; }

        DateTime LimitedDateToShowInfo { get; }

        bool IsAdminCanViewVNIP { get; }

        bool IsEnableSabaPromotion { get; }

        bool IsEnableSabaPromotionForHeadline { get; }
        
        int SubAccId { get; }

        string IP { get; }

        bool IsInternal { get; }

        bool IsProtectedMode { get; }
        
        string DarkModeName { get; }

        string MainDomain { get; }

        bool AutoFollowBetIsDummyAgent { get; }
        
        string MainSessionID { get; }

        string SessionID { get; }

        int OnUserId { get; }
    }
}