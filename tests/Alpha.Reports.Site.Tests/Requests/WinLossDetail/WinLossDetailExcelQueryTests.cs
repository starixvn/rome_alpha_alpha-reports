namespace Alpha.Reports.Site.Tests.Requests.WinLossDetail
{
    using NSubstitute;
    using Reports.Models;
    using Site.Requests.WinLossDetail;
    using Xunit;

    public class WinLossDetailExcelQueryTests
    {
        [Theory]
        [InlineData(4, "~/App_Data/ExcelTemplates/WinLossDetail/Master.xls")]
        [InlineData(3, "~/App_Data/ExcelTemplates/WinLossDetail/Agent.xls")]
        [InlineData(2, "~/App_Data/ExcelTemplates/WinLossDetail/Member.xls")]
        public void BuildExcelTemplatePath_Always_ReturnsCorrectPathByUserLevel(byte userLevelId, string expectedPath)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            var winLossDetailQuery = new WinLossDetailExcelQuery(user);
            winLossDetailQuery.UserLevelId = userLevelId;

            // Act
            var actualPath = winLossDetailQuery.BuildExcelTemplatePath();

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }
    }
}