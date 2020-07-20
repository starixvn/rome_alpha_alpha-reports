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

        [ConfigurationProperty("timePeriod", IsRequired = true)]
        public int TimePeriod
        {
            get
            {
                return (int)this["timePeriod"];
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