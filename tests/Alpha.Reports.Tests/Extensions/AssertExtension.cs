namespace Alpha.Reports.Tests.Extensions
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using Xunit;

    public static class AssertExtension
    {
        public static void Equal<T>(T expected, T actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.Equal(expectedJson, actualJson);
        }

        public static void Equal(string expectedJson, IEnumerable<object> actual)
        {
            var expectedJObject = JArray.Parse(expectedJson);
            var actualJObject = JArray.FromObject(actual);
            Assert.True(JToken.DeepEquals(expectedJObject, actualJObject));
        }
    }
}
