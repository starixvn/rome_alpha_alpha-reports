namespace Alpha.Reports.Tests.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Data;
    using System.IO;

    public static class FileHelper
    {
        public static string GetFilePath(string relativeFilePath)
        {
            string codeBase = typeof(FileHelper).Assembly.CodeBase;
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

        public static T ReadJson<T>(string relativeFilePath, JsonSerializerSettings settings)
        {
            var json = ReadContent(relativeFilePath);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static T ReadJson<T>(string relativeFilePath)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
            return ReadJson<T>(relativeFilePath, settings);
        }

        public static string ReadContent(string relativeFilePath)
        {
            var filePath = GetFilePath(relativeFilePath);
            return File.ReadAllText(filePath);
        }
    }
}
