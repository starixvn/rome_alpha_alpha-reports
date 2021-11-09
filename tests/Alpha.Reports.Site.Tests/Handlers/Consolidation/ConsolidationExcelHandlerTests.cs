namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using Alpha.Reports.Models.WinLossDetail;
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
        private readonly ConsolidationByUsernameWorkbook consolidationByUsernameWorkbook;
        private readonly Workbook<WinLossDetailModel> workbookDetail;

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

            consolidationByUsernameWorkbook = Substitute.For<ConsolidationByUsernameWorkbook>();
            workbookDetail = Substitute.For<Workbook<WinLossDetailModel>>();
            consolidationByDateWorkbook = Substitute.For<ConsolidationByDateWorkbook>();
            consolidationByProductWorkbook = Substitute.For<ConsolidationByProductWorkbook>();
            consolidationExcelHandler = new ConsolidationExcelHandler(
                productService,
                reportService,
                cacheRefService,
                viewer,
                consolidationByUsernameWorkbook,
                workbookDetail,
                consolidationByDateWorkbook,
                consolidationByProductWorkbook,
                productDetailService);

            consolidationExcelHandler.MapPath = (path) => path;
            consolidationExcelHandler.IsFileExist = (path) => true;

            reportService.GetConsolidationByUsername(Arg.Any<GetConsolidationParam>()).Returns(
                new List<ConsolidationByUsername> { new ConsolidationByUsername(1, 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, null, 0) });

            reportService.GetConsolidationByDate(Arg.Any<GetConsolidationParam>()).Returns(
                new List<ConsolidationByDate> { new ConsolidationByDate("2016/12/2", 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) });

            reportService.GetConsolidationByProduct(Arg.Any<GetConsolidationParam>()).Returns(
                new List<ConsolidationByProduct> { new ConsolidationByProduct(1, 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1) });

            productService.ConcatProductNames(Arg.Any<string>()).Returns(callInfo => callInfo.Args()[0]);

            compareLogic = new CompareLogic();

            //consolidationByUsernameWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(consolidationByUsernameWorkbook);
            //consolidationByUsernameWorkbook.Title(Arg.Any<string>()).Returns(consolidationByUsernameWorkbook);
            //consolidationByUsernameWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(consolidationByUsernameWorkbook);
            //consolidationByUsernameWorkbook.Body(Arg.Any<IEnumerable<ConsolidationByUsernameViewModel>>(), Arg.Any<Mapper<ConsolidationByUsernameViewModel>>()).Returns(consolidationByUsernameWorkbook);
            //consolidationByUsernameWorkbook.Sheet(Arg.Any<string>()).Returns(consolidationByUsernameWorkbook);

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

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_Always_UsesCorrectTemplateInfo()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user);

        //    // Act
        //    consolidationExcelHandler.Handle(message);

        //    // Assert
        //    consolidationByUsernameWorkbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
        //        tp => tp.NoInfomationText == "No information is available"
        //        && tp.FreezeColumnName == "C"
        //        && tp.FreezeRowIndex == 3));
        //}

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectTitle()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user)
        //    {
        //        UserSelectedProductIds = "1,33",
        //        UserSelectedCurrencyId = 0,
        //        FromDate = DefaultVariablesTest.CurrentDate
        //    };

        //    // Act
        //    consolidationExcelHandler.Handle(message);

        //    // Assert
        //    consolidationByUsernameWorkbook.Received().Title(Arg.Is<string>(title => title == "Consolidation Report By Username 12/02/2016 --> 12/02/2016 of 1,33 - All Currencies"));
        //}

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_SelectCurrency_UsesCorrectTitle()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user)
        //    {
        //        UserSelectedProductIds = "1,33",
        //        UserSelectedCurrencyId = 1,
        //        FromDate = DefaultVariablesTest.CurrentDate
        //    };

        //    // Act
        //    consolidationExcelHandler.Handle(message);

        //    // Assert
        //    consolidationByUsernameWorkbook.Received().Title(Arg.Is<string>(title => title == "Consolidation Report By Username 12/02/2016 --> 12/02/2016 of 1,33 - Currency: Currency 1"));
        //}

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectHeader()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user);

        //    // Act
        //    consolidationExcelHandler.Handle(message);

        //    // Assert
        //    consolidationByUsernameWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        //}

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectBody()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user);
        //    var expectedExportData = new List<ConsolidationEntityViewModel>
        //    {
        //        new ConsolidationEntityViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1")
        //    };

        //    // Act
        //    consolidationExcelHandler.Handle(message);

        //    // Assert
        //    consolidationByUsernameWorkbook.Received().Body(
        //        Arg.Is<IEnumerable<ConsolidationEntityViewModel>>(reportData => compareLogic.Compare(expectedExportData, reportData).AreEqual),
        //        Arg.Any<Mapper<ConsolidationEntityViewModel>>());
        //}

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_AllCurrency_UsesCorrectSheetName()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user);

        //    // Act
        //    consolidationExcelHandler.Handle(message);

        //    // Assert
        //    consolidationByUsernameWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        //}

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_AllCurrency_ExportsExcelToBytes()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user);

        //    // Act
        //    consolidationExcelHandler.Handle(message);

        //    // Assert
        //    consolidationByUsernameWorkbook.Received().ExportToBytes();
        //}

        //[Fact]
        //public void HandleConsolidationByUsernameExcelQuery_AllCurrency_ExportCorrectFile()
        //{
        //    // Arrange
        //    var message = new ConsolidationByUsernameExcelQuery(user)
        //    {
        //        FromDate = DefaultVariablesTest.CurrentDate
        //    };

        //    consolidationByUsernameWorkbook.ExportToBytes().Returns(new byte[0]);

        //    var expectedFile = new ExcelFileViewModel("Consolidation-12022016-12022016", new byte[0]);

        //    // Act
        //    var actualFile = consolidationExcelHandler.Handle(message);

        //    // Assert
        //    AssertExtension.Equal(expectedFile, actualFile);
        //}

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
                    new List<ConsolidationEntityViewModel> { new ConsolidationEntityViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
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
                    new List<ConsolidationEntityViewModel> { new ConsolidationEntityViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
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
    }
}
