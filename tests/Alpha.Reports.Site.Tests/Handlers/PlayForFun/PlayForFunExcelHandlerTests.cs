namespace Alpha.Reports.Site.Tests.Handlers.PlayForFun
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Services;
    using Site.Handlers.PlayForFun;
    using Site.Requests.PlayForFun;
    using Site.ViewModels.PlayForFun;
    using Utils;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("UserCollection")]
    public class PlayForFunExcelHandlerTests
    {
        private readonly PlayForFunExcelHandler handler;
        private readonly IUser user;
        private readonly Workbook<PlayForFunModel> workbook;
        private readonly CompareLogic compareLogic;

        private readonly IEnumerable<MasterAgentPlayForFunModel> masterAgentPlayForFunModels;
        private readonly IEnumerable<MemberPlayForFunModel> memberPlayForFunModels;

        public PlayForFunExcelHandlerTests(UserFixture userFixture)
        {
            var productService = Substitute.For<ProductService>();
            productService.ConcatProductNames(Arg.Any<string>()).Returns("Product Names");
            var reportService = Substitute.For<PlayForFunService>();

            workbook = Substitute.For<Workbook<PlayForFunModel>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<PlayForFunModel>>(), Arg.Any<Mapper<PlayForFunModel>>()).Returns(workbook);
            workbook.Footer(Arg.Any<PlayForFunModel>(), Arg.Any<Mapper<PlayForFunModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            handler = Substitute.ForPartsOf<PlayForFunExcelHandler>(productService, reportService, workbook);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;

            user = userFixture.User;
            SystemTime.Today = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            var supportedProduct = new List<Product>
            {
                new Product(6, "Casino"),
                new Product(22, "Macau Games"),
                new Product(24, "Fishing World")
            };
            reportService.SupportedProduct(Arg.Any<int>()).Returns(supportedProduct);

            masterAgentPlayForFunModels = new List<MasterAgentPlayForFunModel>
            {
                new MasterAgentPlayForFunModel(0, string.Empty, 1, 2, 3, 4)
            };

            reportService.MasterPlayForFun(Arg.Any<PlayForFunParam>()).Returns(masterAgentPlayForFunModels);
            reportService.AgentPlayForFun(Arg.Any<PlayForFunParam>()).Returns(masterAgentPlayForFunModels);

            memberPlayForFunModels = new List<MemberPlayForFunModel>
            {
                new MemberPlayForFunModel(0, string.Empty, true, 1, 2, 3)
            };

            reportService.MemberPlayForFun(Arg.Any<PlayForFunParam>()).Returns(memberPlayForFunModels);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandleExportMasterExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.FreezeColumnName == "A" &&
                tp.FreezeRowIndex == 2));
        }

        [Fact]
        public void HandleExportMasterExcelQuery_Always_ExportsCorrectTitle()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is("Master Play For Fun Detail 12/02/2016 --> 12/02/2016 of Product Names (All Status)"));
        }

        [Fact]
        public void HandleExportMasterExcelQuery_Always_ExportsCorrectHeader()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportMasterExcelQuery_Always_ExportsCorrectBody()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Body(Arg.Is(masterAgentPlayForFunModels), Arg.Any<Mapper<PlayForFunModel>>());
        }

        [Fact]
        public void HandleExportMasterExcelQuery_Always_ExportsCorrectFooter()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(user);
            var expectedTotal = new MasterAgentPlayForFunModel(0, string.Empty, 1, 2, 3, 4);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Footer(
                Arg.Is<PlayForFunModel>(total => compareLogic.Compare(expectedTotal, total).AreEqual),
                Arg.Any<Mapper<PlayForFunModel>>());
        }

        [Fact]
        public void HandleExportMasterExcelQuery_Always_ExportsCorrectSheetName()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportMasterExcelQuery_Always_ExportsExcelToBytes()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportAgentExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.FreezeColumnName == "A" &&
                tp.FreezeRowIndex == 2));
        }

        [Fact]
        public void HandleExportAgentExcelQuery_Always_ExportsCorrectTitle()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(user) { CustLevelId = 3 };

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is("Agent Play For Fun Detail 12/02/2016 --> 12/02/2016 of Product Names (All Status)"));
        }

        [Fact]
        public void HandleExportAgentExcelQuery_Always_ExportsCorrectHeader()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportAgentExcelQuery_Always_ExportsCorrectBody()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Body(Arg.Is(masterAgentPlayForFunModels), Arg.Any<Mapper<PlayForFunModel>>());
        }

        [Fact]
        public void HandleExportAgentExcelQuery_Always_ExportsCorrectFooter()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(user);
            var expectedTotal = new MasterAgentPlayForFunModel(0, string.Empty, 1, 2, 3, 4);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Footer(
                Arg.Is<PlayForFunModel>(total => compareLogic.Compare(expectedTotal, total).AreEqual),
                Arg.Any<Mapper<PlayForFunModel>>());
        }

        [Fact]
        public void HandleExportAgentExcelQuery_Always_ExportsCorrectSheetName()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportAgentExcelQuery_Always_ExportsExcelToBytes()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportMemberExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.FreezeColumnName == "A" &&
                tp.FreezeRowIndex == 2));
        }

        [Fact]
        public void HandleExportMemberExcelQuery_Always_ExportsCorrectTitle()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is("Member Play For Fun Detail 12/02/2016 --> 12/02/2016 of Product Names (All Status)"));
        }

        [Fact]
        public void HandleExportMemberExcelQuery_Always_ExportsCorrectHeader()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportMemberExcelQuery_Always_ExportsCorrectBody()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Body(Arg.Is(memberPlayForFunModels), Arg.Any<Mapper<PlayForFunModel>>());
        }

        [Fact]
        public void HandleExportMemberExcelQuery_Always_ExportsCorrectFooter()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(user);
            var expectedTotal = new MemberPlayForFunViewModel(0, string.Empty, 1, 2, 3, false, string.Empty);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Footer(
                Arg.Is<MemberPlayForFunViewModel>(total => compareLogic.Compare(expectedTotal, total).AreEqual),
                Arg.Any<Mapper<PlayForFunModel>>());
        }

        [Fact]
        public void HandleExportMemberExcelQuery_Always_ExportsCorrectSheetName()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportMemberExcelQuery_Always_ExportsExcelToBytes()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }
    }
}
