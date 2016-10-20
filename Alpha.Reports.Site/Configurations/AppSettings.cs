namespace Alpha.Reports.Site.Configurations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Web;
    using Models;

    public class AppSettings
    {
        private static IEnumerable<Product> products;

        public static string OldMainAppPath
        {
            get
            {
                return GetConfigValue("OldMainAppPath");
            }
        }

        public static string AccessDeniedPage
        {
            get
            {
                return GetConfigValue("AccessDeniedPage");
            }
        }

        public static string StoreProcedurePath
        {
            get
            {
                return GetConfigValue("StoreProcedurePath");
            }
        }

        public static IEnumerable<Product> Products
        {
            get
            {
                if (products == null)
                {
                    LoadProductConfig();
                }

                return products;
            }
        }

        public static string AssetAppPath
        {
            get
            {
                return GetConfigValue("AssetAppPath");
            }
        }

        private static string ProductConfigPath
        {
            get
            {
                return GetConfigValue("Reports:Products");
            }
        }

        public static string GetConfigValue(string configName)
        {
            return ConfigurationManager.AppSettings[configName];
        }

        public static void LoadProductConfig()
        {
            if (HttpContext.Current == null || HttpContext.Current.Server == null)
            {
                return;
            }

            var configFilePath = HttpContext.Current.Server.MapPath(ProductConfigPath);

            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Cannot find security code json file");
            }

            using (Stream stream = File.OpenRead(configFilePath))
            {
                var serializer = new DataContractJsonSerializer(typeof(IEnumerable<Product>));
                products = (IEnumerable<Product>)serializer.ReadObject(stream);
            }
        }
    }
}