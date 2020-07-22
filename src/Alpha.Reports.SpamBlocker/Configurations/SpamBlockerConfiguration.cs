using System.Configuration;

namespace Alpha.Reports.SpamBlocker.Configurations
{
    public class SpamBlockerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
        }

        [ConfigurationProperty("maxRequests", IsRequired = true)]
        public int MaxRequests
        {
            get
            {
                return (int)this["maxRequests"];
            }
        }

        [ConfigurationProperty("periodInSeconds", IsRequired = true)]
        public int PeriodInSeconds
        {
            get
            {
                return (int)this["periodInSeconds"];
            }
        }

        public static SpamBlockerConfiguration Current
        {
            get
            {
                return ConfigurationManager.GetSection("SpamBlockerConfiguration") as SpamBlockerConfiguration;
            }
        }
    }
}