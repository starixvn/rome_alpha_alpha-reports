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

        [ConfigurationProperty("perSeconds", IsRequired = true)]
        public int PerSeconds
        {
            get
            {
                return (int)this["perSeconds"];
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