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

        bool GetHideProduct(int productId);

        DateTime LimitedDateToShowInfo { get; }

        bool IsAdminCanViewVNIP { get; }
    }
}