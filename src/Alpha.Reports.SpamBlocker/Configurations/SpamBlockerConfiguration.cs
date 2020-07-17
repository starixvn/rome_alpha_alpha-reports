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

        [ConfigurationProperty("timer", IsRequired = true)]
        public int Timer
        {
            get
            {
                return (int)this["timer"];
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