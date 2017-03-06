namespace Alpha.Reports.Site.Tests.Helpers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Site.Helpers;
    using Site.Models;
    using Site.Requests;
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

        protected override object CallBaseBindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Simulate case when model is not ReportBaseQuery
            if (bindingContext != null)
            {
                return "ModelIsNotReportBaseQuery";
            }

            var reportBaseQuery = new ReportBaseQuery();

            return reportBaseQuery;
        }

        protected override string GetBreadcrumbLevelJson(ControllerContext controllerContext)
        {
            if (controllerContext != null)
            {
                return ",[{'CustId':1176070,'CustName':'SUPERTEST1','CustLevelId':4}]";
            }

            return "[{'CustId':1176070,'CustName':'SUPERTEST1','CustLevelId':4}]";
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

        [Fact]
        public void BindModel_ModelIsNotReportBaseQuery_ReturnStubData()
        {
            // Act
            var model = BindModel(null, new ModelBindingContext());

            // Assert
            Assert.Equal("ModelIsNotReportBaseQuery", model);
        }

        [Fact]
        public void BindModel_ModelIsReportBaseQuery_ReturnReportBaseQuery()
        {
            // Act
            var model = BindModel(null, null);

            // Assert
            Assert.True(model is ReportBaseQuery);
        }

        [Fact]
        public void BindModel_BreadcrumbLevelJsonHasData_ReturnDeserializedBreadcrumLevel()
        {
            // Act
            var breadcrumLevel = (BindModel(null, null) as ReportBaseQuery).BreadcrumbLevel;

            // Assert
            var superInfo = breadcrumLevel.ElementAt(0);
            Assert.True(breadcrumLevel.Count() == 1
                && superInfo.CustName == "SUPERTEST1"
                && superInfo.CustLevelId == 4
                && superInfo.CustId == 1176070);
        }

        [Fact]
        public void BindModel_BreadcrumbLevelJsonHasValidData_ReturnEmptyBreadcrumbLevel()
        {
            // Arrange
            var controllerContext = new ControllerContext();

            // Act
            var breadcrumbLevel = (BindModel(controllerContext, null) as ReportBaseQuery).BreadcrumbLevel;

            // Assert
            Assert.Empty(breadcrumbLevel);
        }
    }
}