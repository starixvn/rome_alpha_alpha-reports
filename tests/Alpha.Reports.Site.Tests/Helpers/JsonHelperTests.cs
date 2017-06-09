using Alpha.Reports.Site.Helpers;
using Xunit;

namespace Alpha.Reports.Site.Tests.Helpers
{
    public class JsonHelperTests
    {
        [Fact]
        public void ToJson_Always_UseSingleQuotationsMarks()
        {
            // Arrange
            var data = new { Username = "Username", CustId = 1 };
            var expectedJson = "{'Username':'Username','CustId':1}";
            JsonHelper.ToJsonFunc = null;

            // Act
            var actualJson = JsonHelper.ToJsonFunc(data);

            // Assert
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
