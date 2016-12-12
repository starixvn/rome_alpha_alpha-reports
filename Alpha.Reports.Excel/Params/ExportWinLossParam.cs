namespace Alpha.Reports.Excel.Params
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Models;

    public class ExportWinLossParam
    {
        public ExportWinLossParam(
            UserLevel userLevel,
            string userSelectedProductIds)
        {
            UserLevel = userLevel;
            UserSelectedProductIds = string.IsNullOrWhiteSpace(userSelectedProductIds)
                ? Enumerable.Empty<int>()
                : userSelectedProductIds.Split(',').Select(s => Converter.ToInt(s));
        }

        public UserLevel UserLevel { get; }

        public IEnumerable<int> UserSelectedProductIds { get; }
    }
}
