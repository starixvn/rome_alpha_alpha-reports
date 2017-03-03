////namespace Alpha.Reports.Site.Tests.Controllers
////{
////    using System.Web.Mvc;
////    using Fixtures;
////    using MediatR;
////    using NSubstitute;
////    using Site.Controllers;
////    using Site.Requests.WinLossByBetType;
////    using Site.ViewModels;
////    using Xunit;

////    [Trait("Category", "WinLossByBetType")]
////    [Collection("UserCollection")]
////    public class WinLossByBetTypeControllerTests
////    {
////        private readonly IMediator mediator;
////        private readonly WinLossByBetTypeController controller;
////        private readonly WinLossByBetTypeQuery winLossByBetTypeQuery;
////        private readonly WinLossByBetTypeExcelQuery winLossByBetTypeExcelQuery;
////        private readonly UserFixture userFixture;

////        public WinLossByBetTypeControllerTests(UserFixture userFixture)
////        {
////            mediator = Substitute.For<IMediator>();
////            this.userFixture = userFixture;
////            controller = new WinLossByBetTypeController(mediator);
////            winLossByBetTypeQuery = new WinLossByBetTypeQuery(this.userFixture.User);
////            winLossByBetTypeExcelQuery = new WinLossByBetTypeExcelQuery(this.userFixture.User);
////        }

////        [Fact]
////        public void WinLossByBettype_Always_ReturnIndex()
////        {
////            // Arrange

////            // Act
////            ViewResult result = controller.Index(winLossByBetTypeQuery) as ViewResult;

////            // Assert
////            Assert.NotNull(result);
////        }

////        [Fact]
////        public void WinLossByBettype_Always_CallsMediator()
////        {
////            // Arrange

////            // Act
////            controller.Index(winLossByBetTypeQuery);

////            // Assert
////            mediator.Received().Send(Arg.Any<WinLossByBetTypeQuery>());
////        }

////        [Fact]
////        public void WinLossByBettype_CallsMediator_MustSendWinLossByBettypeQuery()
////        {
////            // Arrange

////            // Act
////            controller.Index(winLossByBetTypeQuery);

////            // Assert
////            mediator.Received().Send(Arg.Is(winLossByBetTypeQuery));
////        }

////        [Fact]
////        public void WinLossByBettypeExcel_Always_CallsMediatorSendExportWinLossQuery()
////        {
////            // Arrange
////            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossByBetTypeExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

////            // Act
////            controller.Excel(winLossByBetTypeExcelQuery);

////            // Assert
////            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(winLossByBetTypeExcelQuery));
////        }
////    }
////}