namespace Alpha.Reports.Site.Tests.Handlers.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models;
    using Reports.Models.ProgressiveContribution;
    using Services;
    using Site.Handlers.ProgressiveContribution;
    using Site.Requests.ProgressiveContribution;
    using Xunit;

    [Trait("Category", "ProgressiveGamesContribution")]
    [Collection("UserCollection")]
    public class ProgressiveContributionExcelHandlerTests
    {
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;
        private readonly ProductService productService;
        private readonly ProgressiveContributionService progressiveContributionService;
        private readonly ProgressiveContributionWorkbook progressiveContributionWorkbook;
        private readonly Workbook<ProgressiveContributionByBetTypeModel> onebookWorkbook;
        private readonly ProgressiveContributionExcelHandler excelHandler;

        public ProgressiveContributionExcelHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            compareLogic = new CompareLogic();

            productService = Substitute.For<ProductService>();
            productService.ConcatProductNames(Arg.Any<string>()).Returns("Product Name");

            progressiveContributionService = Substitute.For<ProgressiveContributionService>();
            progressiveContributionWorkbook = Substitute.For<ProgressiveContributionWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<ProgressiveContributionByBetTypeModel>>();

            progressiveContributionWorkbook.SetData(
                Arg.Any<IEnumerable<ProgressiveContributionByProductModel>>(),
                Arg.Any<ProgressiveContributionByBetTypeModel>())
                .Returns(progressiveContributionWorkbook);

            progressiveContributionWorkbook.SetRowMappers(
                Arg.Any<Mapper<ProgressiveContributionByBetTypeModel>>(),
                Arg.Any<Mapper<ProgressiveContributionByBetTypeModel>>())
                .Returns(progressiveContributionWorkbook);

            progressiveContributionWorkbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Footer(
                Arg.Any<IEnumerable<ProgressiveContributionByBetTypeModel>>(),
                Arg.Any<Mapper<ProgressiveContributionByBetTypeModel>>())
                .Returns(onebookWorkbook);

            excelHandler = Substitute.ForPartsOf<ProgressiveContributionExcelHandler>(
                productService,
                progressiveContributionService,
                progressiveContributionWorkbook);
            excelHandler.MapPath = (path) => path;
            excelHandler.IsFileExist = (path) => true;
        }

        [Fact]
        public void Handle_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            ProgressiveContributionExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => excelHandler.Handle(nullMessage));
        }

        [Fact]
        public void Handle_NotNullMessage_SetDataToWorkbook()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);

            // Act
            excelHandler.Handle(message);

            // Assert
            progressiveContributionWorkbook.Received().SetData(
                Arg.Any<IEnumerable<ProgressiveContributionByProductModel>>(),
                Arg.Any<ProgressiveContributionByBetTypeModel>());
        }

        [Fact]
        public void Handle_NotNullMessage_SetRowMappers()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);

            // Act
            excelHandler.Handle(message);

            // Assert
            progressiveContributionWorkbook.Received().SetRowMappers(
                Arg.Any<Mapper<ProgressiveContributionByBetTypeModel>>(),
                Arg.Any<Mapper<ProgressiveContributionByBetTypeModel>>());
        }

        [Fact]
        public void Handle_NotNullMessage_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);

            // Act
            excelHandler.Handle(message);

            // Assert
            progressiveContributionWorkbook.Received().UseTemplate(
                Arg.Is<TemplateInfo>(tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void Handle_NotNullMessage_ExportsCorrectTitle()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);
            message.FromDate = DefaultVariablesTest.CurrentDate;
            message.ToDate = DefaultVariablesTest.CurrentDate;

            // Act
            excelHandler.Handle(message);

            // Assert
            onebookWorkbook.Received().Title(
                Arg.Is("Progressive Games Contribution 12/02/2016 --> 12/02/2016 of Product Name"));
        }

        [Fact]
        public void Handle_NotNullMessage_ExportsCorrectHeader()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);

            // Act
            excelHandler.Handle(message);

            // Assert
            onebookWorkbook.Received().Header(Arg.Is<Func<string, string>>(
                func => func("BetTypeName") == "Product/Bet Type"));
        }

        [Fact]
        public void Handle_NotNullMessage_ExportsCorrectSheetName()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);

            // Act
            excelHandler.Handle(message);

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void Handle_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);
            message.CustLevelId = custLevelId;

            // Act
            excelHandler.Handle(message);

            // Assert
            excelHandler.Received().GetExcelTemplatePath(fileName);
        }

        [Fact]
        public void GetRowMapper_DataRow_MapColumnAWithCorrectBetTypeName()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);
            var viewModel = new ProgressiveContributionByBetTypeModel(1, 2, "BetTypeName", 1000, 100, -10, -20, -30);

            // Act
            var actualMapper = excelHandler.GetRowMapper(message, false);

            // Assert
            Assert.Equal("BetTypeName", (string)actualMapper.Mappings[Column.A](viewModel));
        }

        [Fact]
        public void GetRowMapper_DataRow_MapColumnAWithTotalLabel()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);
            var viewModel = new ProgressiveContributionByBetTypeModel(1, 2, null, 1000, 100, -10, -20, -30);

            // Act
            var actualMapper = excelHandler.GetRowMapper(message, true);

            // Assert
            Assert.Equal("Total", (string)actualMapper.Mappings[Column.A](viewModel));
        }

        [Fact]
        public void GetRowMapper_AgentLevel_ReturnCorrectMapper()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new ProgressiveContributionExcelQuery(user);
            var viewModel = new ProgressiveContributionByBetTypeModel(1, 2, "BetTypeName", 1000, 100, -10, 0, 0);

            // Act
            var actualMapper = excelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.D](viewModel) == -10 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 10);
        }

        [Fact]
        public void GetRowMapper_MasterLevel_ReturnCorrectMapper()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)3);
            var message = new ProgressiveContributionExcelQuery(user);
            var viewModel = new ProgressiveContributionByBetTypeModel(1, 2, "BetTypeName", 1000, 100, -10, -20, 0);

            // Act
            var actualMapper = excelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.D](viewModel) == -20 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == -10 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 30);
        }

        [Fact]
        public void GetRowMapper_SuperLevel_ReturnCorrectMapper()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            var message = new ProgressiveContributionExcelQuery(user);
            var viewModel = new ProgressiveContributionByBetTypeModel(1, 2, "BetTypeName", 1000, 100, -10, -20, -30);

            // Act
            var actualMapper = excelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.D](viewModel) == -30 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == -20 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == -10 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 60);
        }
    }
}