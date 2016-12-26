namespace Alpha.Reports.Site.Adapters
{
    using System;
    using System.Configuration;

    public static class ConfigurationManagerAdapter
    {
        [ThreadStatic]
        private static Func<string, object> getSection;

        public static Func<string, object> GetSection
        {
            get { return getSection ?? ConfigurationManager.GetSection; }
            set { getSection = value; }
        }
    }
}