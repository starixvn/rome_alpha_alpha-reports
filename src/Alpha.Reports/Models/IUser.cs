namespace Alpha.Reports.Models
{
    using System.Collections.Generic;

    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        byte LevelId { get; }

        string Language { get; }

        string LanguageKey { get; }

        string CurrencyName { get; }

        bool IsShowConsolidatedReport { get; }

        bool IsEnabledJackpot { get; }

        IList<int> EnableJackpotProductIds { get; }

        IList<int> GetEnableProductsForGame(string gameName);

        bool GetHideProduct(int productId);
    }
}