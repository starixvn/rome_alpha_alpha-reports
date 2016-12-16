namespace Alpha.Reports.Excel.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Enumerations;
    using Excel.Helpers;
    using Excel.Params;
    using Excel.Services.Impl;
    using Models;
    using Models.WinlossDetails;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Services;
    using Xunit;

    public class WinLossDetailExcelExporterImplTests
    {
        private readonly WinLossDetailExcelExporterImpl winLossDetailExcelExporterImplTests;

        private ProductService productService;

        private Workbook<WinLossDetail> workbook;

        public WinLossDetailExcelExporterImplTests()
        {
            productService = Substitute.For<ProductService>();
            workbook = Substitute.For<Workbook<WinLossDetail>>();

            winLossDetailExcelExporterImplTests = new WinLossDetailExcelExporterImpl(productService, workbook);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void Export_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailExcelExporterImplTests.Export(nullParam, null));
        }

        [Fact]
        public void Export_NullWinLossDetailParam_ThrowsArgumentNullException()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>();
            ExportWinLossDetailParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailExcelExporterImplTests.Export(exportExcelParam, nullParam));
        }

        [Fact]
        public void Export_MasterWinLossDetail_UsesCorrectTemplateInfo()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Master Win Loss Detail",
                SheetName = "Master Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), string.Empty);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.TemplatePath == exportExcelParam.TemplatePath &&
                tp.NoInfomationText == "No information is available" &&
                tp.FreezeColumnName == "B" &&
                tp.FreezeRowIndex == 3));
        }

        [Fact]
        public void Export_MasterWinLossDetail_ExportsCorrectTitle()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Master Win Loss Detail",
                SheetName = "Master Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().Title(Arg.Is<string>(s => string.Equals(s, "Master Win Loss Detail 12/02/2016 --> 12/02/2016 of Product Name")));
        }

        [Fact]
        public void Export_MasterWinLossDetail_ExportsCorrectHeader()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Master Win Loss Detail",
                SheetName = "Master Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().Header(Arg.Is<Func<string, string>>(func => func == ExcelHelper.HeaderTitleMapping));
        }

        [Fact]
        public void Export_MasterWinLossDetail_ExportsCorrectBody()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Master Win Loss Detail",
                SheetName = "Master Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().Body(
                Arg.Is<IEnumerable<WinLossDetail>>(dataSource => dataSource == exportExcelParam.ReportData.DataSource),
                Arg.Any<Mapper<WinLossDetail>>());
        }

        [Fact]
        public void Export_MasterWinLossDetail_ExportsCorrectFooter()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Master Win Loss Detail",
                SheetName = "Master Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<WinLossDetail>>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().Footer(Arg.Is<WinLossDetail>(total => total == exportExcelParam.ReportData.TotalData), Arg.Any<Mapper<WinLossDetail>>());
        }

        [Fact]
        public void Export_MasterWinLossDetail_ExportsCorrectSheetName()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Master Win Loss Detail",
                SheetName = "Master Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<WinLossDetail>>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Footer(Arg.Any<WinLossDetail>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().Sheet(Arg.Is<string>(sheetName => sheetName == exportExcelParam.SheetName));
        }

        [Fact]
        public void Export_MasterWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Master Win Loss Detail",
                SheetName = "Master Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<WinLossDetail>>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Footer(Arg.Any<WinLossDetail>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Export_AgentWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Agent Win Loss Detail",
                SheetName = "Agent Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(3), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<WinLossDetail>>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Footer(Arg.Any<WinLossDetail>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Export_MemberWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            ExportExcelParam<WinLossDetail> exportExcelParam = new ExportExcelParam<WinLossDetail>
            {
                TemplatePath = "TemplatePath",
                ReportName = "Member Win Loss Detail",
                SheetName = "Member Win Loss Detail",
                ReportData = new ReportData<WinLossDetail>(new List<WinLossDetail>(), 0, 0, 0, new WinLossDetail()),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            ExportWinLossDetailParam exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(2), "1");
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<WinLossDetail>>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Footer(Arg.Any<WinLossDetail>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            // Act
            winLossDetailExcelExporterImplTests.Export(exportExcelParam, exportWinLossDetailParam);

            // Assert
            workbook.Received().ExportToBytes();
        }
    }
}
