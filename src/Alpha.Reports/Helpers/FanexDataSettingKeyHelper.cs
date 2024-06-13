namespace Alpha.Reports.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Configurations;

    public static class FanexDataSettingKeyHelper
    {
        public static string GetWinLossDetailSettingKeySubfix(string siteName)
            => WinLossReportSettingKeys.TryGetValue(siteName, out var subfix) ? subfix : string.Empty;

        private static IDictionary<string, string> winLossDetailSettingKeys;
        internal static IDictionary<string, string> WinLossReportSettingKeys
        {
            get
            {
                if (winLossDetailSettingKeys == null)
                {
                    winLossDetailSettingKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    (SiteSettings.WLDetailSettingKeySites() ?? string.Empty)
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList()
                        .ForEach(item =>
                        {
                            var siteAndSubfix = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                            if (siteAndSubfix.Length == 2)
                            {
                                winLossDetailSettingKeys[siteAndSubfix[0]] = $"_{siteAndSubfix[1]}";
                            }
                        });
                }

                return winLossDetailSettingKeys;
            }
        }
    }
}
