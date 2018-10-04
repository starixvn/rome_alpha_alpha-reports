namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Extensions;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params.Consolidation;
    using Reports.Models.Consolidation;
    using Site.Handlers.Consolidation;
    using Site.Requests.Consolidation;
    using Site.ViewModels;
    using Site.ViewModels.Consolidation;
    using Utils;
    using Xunit;

    [Trait("Category", "Consolidation")]
    [Collection("UserCollection")]
    public class ConsolidationExcelHandlerTests : ConsolidationTestsBaseFunctions
    {
        private readonly Workbook<ConsolidationByUsernameViewModel> workbook;

        private readonly ConsolidationByDateWorkbook consolidationByDateWorkbook;
        private readonly Workbook<ConsolidationByDateViewModel> workbookByDate;

        private readonly ConsolidationByProductWorkbook consolidationByProductWorkbook;
        private readonly Workbook<ConsolidationByProductViewModel> workbookByProduct;

        private readonly ConsolidationExcelHandler consolidationExcelHandler;
        private readonly CompareLogic compareLogic;

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ConsolidationExcelHandlerTests(UserFixture userFixture) : base(userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            workbook = Substitute.For<Workbook<ConsolidationByUsernameViewModel>>();
            consolidationByDateWorkbook = Substitute.For<ConsolidationByDateWorkbook>();
            consolidationByProductWorkbook = Substitute.For<ConsolidationByProductWorkbook>();
            consolidationExcelHandler = new ConsolidationExcelHandler(
                productService,
                reportService,
                cacheRefService,
                winLossDetailService,
                viewer,
                workbook,
                consolidationByDateWorkbook,
                consolidationByProductWorkbook);

            consolidationExcelHandler.MapPath = (path) => path;
            consolidationExcelHandler.IsFileExist = (path) => true;

            reportService.GetConsolidationByUsername(Arg.Any<GetConsolidationParam>()).Returns(
                new List<ConsolidationByUsername> { new ConsolidationByUsername(1, 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) });

            reportService.GetConsolidationByDate(Arg.Any<GetConsolidationParam>()).Returns(
                new List<ConsolidationByDate> { new ConsolidationByDate("2016/12/2", 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) });

            reportService.GetConsolidationByProduct(Arg.Any<GetConsolidationParam>()).Returns(
                new List<ConsolidationByProduct> { new ConsolidationByProduct(1, 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) });

            productService.ConcatProductNames(Arg.Any<string>()).Returns(callInfo => callInfo.Args()[0]);

            compareLogic = new CompareLogic();

            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<ConsolidationByUsernameViewModel>>(), Arg.Any<Mapper<ConsolidationByUsernameViewModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            workbookByDate = Substitute.For<Workbook<ConsolidationByDateViewModel>>();
            consolidationByDateWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbookByDate);
            workbookByDate.Title(Arg.Any<string>()).Returns(workbookByDate);
            workbookByDate.Header(Arg.Any<Func<string, string>>()).Returns(workbookByDate);
            workbookByDate.Body(Arg.Any<IEnumerable<ConsolidationByDateViewModel>>(), Arg.Any<Mapper<ConsolidationByDateViewModel>>()).Returns(workbookByDate);
            workbookByDate.Sheet(Arg.Any<string>()).Returns(workbookByDate);

            workbookByProduct = Substitute.For<Workbook<ConsolidationByProductViewModel>>();
            consolidationByProductWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbookByProduct);
            workbookByProduct.Title(Arg.Any<string>()).Returns(workbookByProduct);
            workbookByProduct.Header(Arg.Any<Func<string, string>>()).Returns(workbookByProduct);
            workbookByProduct.Body(Arg.Any<IEnumerable<ConsolidationByProductViewModel>>(), Arg.Any<Mapper<ConsolidationByProductViewModel>>()).Returns(workbookByProduct);
            workbookByProduct.Sheet(Arg.Any<string>()).Returns(workbookByProduct);
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"
                && tp.FreezeColumnName == "C"
                && tp.FreezeRowIndex == 3));
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectTitle()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user)
            {
                UserSelectedProductIds = "1,33",
                UserSelectedCurrencyId = 0,
                UserSelectedExcludeProductIds = "33",
                FromDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is<string>(title => title == "Consolidation Report By Username 12/02/2016 --> 12/02/2016 of 1,33 - All Currencies"));
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_SelectCurrency_UsesCorrectTitle()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user)
            {
                UserSelectedProductIds = "1,33",
                UserSelectedCurrencyId = 1,
                UserSelectedExcludeProductIds = "33",
                FromDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is<string>(title => title == "Consolidation Report By Username 12/02/2016 --> 12/02/2016 of 1,33 - Currency: Currency 1"));
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectHeader()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectBody()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user);
            var expectedExportData = new List<ConsolidationByUsernameViewModel>
            {
                new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1")
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbook.Received().Body(
                Arg.Is<IEnumerable<ConsolidationByUsernameViewModel>>(reportData => compareLogic.Compare(expectedExportData, reportData).AreEqual),
                Arg.Any<Mapper<ConsolidationByUsernameViewModel>>());
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectSheetName()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_AllCurrency_ExportsExcelToBytes()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleConsolidationByUsernameExcelQuery_AllCurrency_ExportCorrectFile()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user)
            {
                FromDate = DefaultVariablesTest.CurrentDate
            };

            workbook.ExportToBytes().Returns(new byte[0]);

            var expectedFile = new ExcelFileViewModel("Consolidation-12022016-12022016", new byte[0]);

            // Act
            var actualFile = consolidationExcelHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedFile, actualFile);
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            consolidationByDateWorkbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"
                && tp.FreezeColumnName == "C"
                && tp.FreezeRowIndex == 3));
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_AllCurrency_UsesCorrectTitle()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user)
            {
                UserSelectedProductIds = "1,33",
                UserSelectedCurrencyId = 0,
                UserSelectedExcludeProductIds = "33",
                FromDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByDate.Received().Title(Arg.Is<string>(title => title == "Daily Consolidated Report 12/02/2016 --> 12/02/2016 of 1,33 - All Currencies"));
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_SelectCurrency_UsesCorrectTitle()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user)
            {
                UserSelectedProductIds = "1,33",
                UserSelectedCurrencyId = 1,
                UserSelectedExcludeProductIds = "33",
                FromDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByDate.Received().Title(Arg.Is<string>(title => title == "Daily Consolidated Report 12/02/2016 --> 12/02/2016 of 1,33 - Currency: Currency 1"));
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_AllCurrency_UsesCorrectHeader()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByDate.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_AllCurrency_UsesCorrectBody()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user);
            var expectedExportData = new List<ConsolidationByDateViewModel>
            {
                new ConsolidationByDateViewModel(
                    DefaultVariablesTest.CurrentDate,
                    new List<ConsolidationByUsernameViewModel> { new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByDate.Received().Body(
                Arg.Is<IEnumerable<ConsolidationByDateViewModel>>(reportData => compareLogic.Compare(expectedExportData, reportData).AreEqual),
                Arg.Any<Mapper<ConsolidationByDateViewModel>>());
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_AllCurrency_UsesCorrectSheetName()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByDate.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_AllCurrency_ExportsExcelToBytes()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByDate.Received().ExportToBytes();
        }

        [Fact]
        public void HandleConsolidationByDateExcelQuery_AllCurrency_ExportCorrectFile()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user)
            {
                FromDate = DefaultVariablesTest.CurrentDate
            };

            workbookByDate.ExportToBytes().Returns(new byte[0]);

            var expectedFile = new ExcelFileViewModel("Consolidation-12022016-12022016", new byte[0]);

            // Act
            var actualFile = consolidationExcelHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedFile, actualFile);
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            consolidationByProductWorkbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"
                && tp.FreezeColumnName == "C"
                && tp.FreezeRowIndex == 3));
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_AllCurrency_UsesCorrectTitle()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user)
            {
                UserSelectedProductIds = "1,33",
                UserSelectedCurrencyId = 0,
                UserSelectedExcludeProductIds = "33",
                FromDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByProduct.Received().Title(Arg.Is<string>(title => title == "Sport Consolidation Report 12/02/2016 --> 12/02/2016 of 1,33 - All Currencies"));
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_SelectCurrency_UsesCorrectTitle()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user)
            {
                UserSelectedProductIds = "1,33",
                UserSelectedCurrencyId = 1,
                UserSelectedExcludeProductIds = "33",
                FromDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByProduct.Received().Title(Arg.Is<string>(title => title == "Sport Consolidation Report 12/02/2016 --> 12/02/2016 of 1,33 - Currency: Currency 1"));
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_AllCurrency_UsesCorrectHeader()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByProduct.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_AllCurrency_UsesCorrectBody()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user);
            var expectedExportData = new List<ConsolidationByProductViewModel>
            {
                new ConsolidationByProductViewModel(
                    1,
                    "Product 1",
                    new List<ConsolidationByUsernameViewModel> { new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
            };

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByProduct.Received().Body(
                Arg.Is<IEnumerable<ConsolidationByProductViewModel>>(reportData => compareLogic.Compare(expectedExportData, reportData).AreEqual),
                Arg.Any<Mapper<ConsolidationByProductViewModel>>());
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_AllCurrency_UsesCorrectSheetName()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByProduct.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_AllCurrency_ExportsExcelToBytes()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user);

            // Act
            consolidationExcelHandler.Handle(message);

            // Assert
            workbookByProduct.Received().ExportToBytes();
        }

        [Fact]
        public void HandleConsolidationByProductExcelQuery_AllCurrency_ExportCorrectFile()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user)
            {
                FromDate = DefaultVariablesTest.CurrentDate
            };

            workbookByProduct.ExportToBytes().Returns(new byte[0]);

            var expectedFile = new ExcelFileViewModel("Consolidation-12022016-12022016", new byte[0]);

            // Act
            var actualFile = consolidationExcelHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedFile, actualFile);
        }

        [Fact]
        public void HandleConsolidationDetailExcelQuery_Alway_ReturnsCorrectData()
        {
            // Arrange
            var message = new ConsolidationDetailExcelQuery(viewer);

            // Act
            var actualData = consolidationExcelHandler.Handle(message);

            // Assert
            AssertExtension.Equal(winLossDetailData, actualData);
        }

        [Fact]
        public void GetRowMapper_Always_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new ConsolidationByUsernameViewModel(1, 2, "Username", 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, "UUS");

            // Act
            var actualMapper = consolidationExcelHandler.GetRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "Agent" &&
                (string)actualMapper.Mappings[Column.B](viewModel) == "Username" &&
                (string)actualMapper.Mappings[Column.C](viewModel) == "UUS" &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 4 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 5 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 6 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 7 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == 8 &&
                (decimal)actualMapper.Mappings[Column.I](viewModel) == 9 &&
                (decimal)actualMapper.Mappings[Column.J](viewModel) == 10 &&
                (decimal)actualMapper.Mappings[Column.K](viewModel) == 11 &&
                (decimal)actualMapper.Mappings[Column.L](viewModel) == 12 &&
                (int)actualMapper.Mappings[Column.M](viewModel) == 13);
        }
    }
}
