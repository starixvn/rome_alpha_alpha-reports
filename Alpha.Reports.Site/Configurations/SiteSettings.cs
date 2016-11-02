namespace Alpha.Reports.Site.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Fanex.Common;
    using Models;
    using Newtonsoft.Json;

    public static class SiteSettings
    {
        private static IEnumerable<Product> products;        

        public static string OldMainAppPath => GetConfigValue("OldMainAppPath");

        public static string AccessDeniedPage => GetConfigValue("AccessDeniedPage");

        public static string StoreProcedurePath => GetConfigValue("StoreProcedurePath");

        public static bool IsShowKenoSetting => Converter.ToBool(GetConfigValue("IsShowKenoSetting"));

        public static string AssetAppPath => GetConfigValue("AssetAppPath");

        private static string ProductConfigPath => GetConfigValue("Reports:Products");               

        public static IEnumerable<Product> Products => products;        

        public static string GetConfigValue(string configName) => ConfigurationManager.AppSettings[configName];

        public static void LoadSettings()
        {
            products = LoadConfig<IEnumerable<Product>>(ProductConfigPath);            
        }

        public static T LoadConfig<T>(string configPath)
        {
            if (HttpContext.Current?.Server == null)
            {
                return default(T);
            }

            var configFilePath = HttpContext.Current.Server.MapPath(configPath);

            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Cannot find json config file: " + configPath);
            }

            T result;

            using (StreamReader stream = File.OpenText(configFilePath))
            {
                result = JsonConvert.DeserializeObject<T>(stream.ReadToEnd());
            }

            return result;
        }

        public static void Verify()
        {
            CheckNullOrWhiteSpace(OldMainAppPath);
            CheckNullOrWhiteSpace(StoreProcedurePath);
            CheckNullOrWhiteSpace(AssetAppPath);
            CheckNullOrWhiteSpace(ProductConfigPath);            

            if (Products?.Any() == false)
            {
                throw new InvalidOperationException($"{nameof(Products)} is null empty");
            }           
        }

        private static void CheckNullOrWhiteSpace(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidOperationException($"{nameof(path)} is null or whitespace");
            }
        }
    }
}