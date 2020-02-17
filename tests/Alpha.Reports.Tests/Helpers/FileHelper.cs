namespace Alpha.Reports.Tests.Helpers
{
    using JsonNet.ContractResolvers;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.IO;
    using System.Reflection;

    public static class FileHelper
    {
        public static string GetFilePath(string relativeFilePath)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(path) + relativeFilePath;
        }

        public static DataSet LoadDataSet(string relativeFilePath)
        {
            var dataSet = new DataSet();
            dataSet.ReadXml(GetFilePath(relativeFilePath));

            return dataSet;
        }

        public static T ReadJson<T>(string relativeFilePath)
        {
            var filePath = GetFilePath(relativeFilePath);
            var json = File.ReadAllText(filePath);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static string ReadJson(string relativeFilePath)
        {
            var filePath = GetFilePath(relativeFilePath);
            return File.ReadAllText(filePath);
        }
    }
}
