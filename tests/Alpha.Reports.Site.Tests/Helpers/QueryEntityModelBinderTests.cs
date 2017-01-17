namespace Alpha.Reports.Site.Tests.Helpers
{
    using System;
    using System.Web.Mvc;
    using Site.Helpers;
    using Site.Models;
    using Site.Requests.WinLossDetail;
    using Xunit;

    [Trait("Category", "Site.Helpers")]
    public class QueryEntityModelBinderTests : QueryEntityModelBinder
    {
        protected override object CreateInstanceOfAssignableReportBaseQuery(Type modelType)
        {
            return "CreateInstanceOfAssignableReportBaseQuery";
        }

        protected override object CallBaseCreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return "CallBaseCreateModel";
        }

        [Fact]
        public void CreateModel_ConstructorParamIsAssignableFromReportBaseQuery_CallCreateInstanceOfAssignableReportBaseQuery()
        {
            // Arrange
            var winLossDetailQueryType = typeof(WinLossDetailQuery);

            // Act
            var createdInstance = CreateModel(null, null, winLossDetailQueryType);

            // Assert
            Assert.Equal("CreateInstanceOfAssignableReportBaseQuery", createdInstance);
        }

        [Fact]
        public void CreateModel_ConstructorParamIsNotAssignableFromReportBaseQuery_CallBaseCreateModel()
        {
            // Arrange
            var userType = typeof(User);

            // Act
            var createdInstance = CreateModel(null, null, userType);

            // Assert
            Assert.Equal("CallBaseCreateModel", createdInstance);
        }
    }
}