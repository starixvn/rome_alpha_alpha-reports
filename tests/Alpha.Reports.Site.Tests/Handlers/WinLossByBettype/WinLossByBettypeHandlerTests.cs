//namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
//{
//    using System;
//    using System.Linq;
//    using Fixtures;
//    using KellermanSoftware.CompareNetObjects;
//    using Reports.Models.WinlossByBettype;
//    using Site.Handlers.WinLossByBettype;
//    using Site.Requests.WinLossByBettype;
//    using Site.ViewModels.WinLossByBettype;
//    using Xunit;

//    [Trait("Category", "WinLossByBettype")]
//    [Collection("UserCollection")]
//    public class WinLossByBettypeHandlerTests
//    {
//        private readonly WinLossByBettypeHandler handler;
//        private readonly WinLossByBettypeQuery message;
//        private CompareLogic compareLogic;

//        public WinLossByBettypeHandlerTests(UserFixture userFixture)
//        {
//            handler = new WinLossByBettypeHandler();
//            compareLogic = new CompareLogic();
//            message = new WinLossByBettypeQuery(userFixture.User);
//        }

//        [Fact]
//        public void HandlerWinLossByBettypeQuery_NullMessage_ThrowsArgumentNullException()
//        {
//            // Arrange
//            WinLossByBettypeQuery nullMessage = null;

//            // Act &  Assert
//            Assert.Throws<ArgumentNullException>(() => handler.Handle(nullMessage));
//        }

//        ////[Fact]
//        ////public void HandlerWinLossByBettypeQuery_InValidMessage_ReturnsDefaultViewModel()
//        ////{
//        ////    // Arrange
//        ////    var reportData = Enumerable.Empty<WinlossByBettypeModel>();
//        ////    var grandTotal = new WinlossByBettypeModel();
//        ////    var expectedViewModel = new WinLossByBettypeViewModel(reportData, grandTotal, message);

//        ////    // Act
//        ////    var actualViewModel = handler.Handle(message);

//        ////    // Act &  Assert
//        ////    var result = compareLogic.Compare(expectedViewModel, actualViewModel);
//        ////    Assert.True(result.AreEqual);
//        ////}

//        [Fact]
//        public void HandlerWinLossByBettypeQuery_ValidMessage_ReturnsCorrectViewModel()
//        {
//            // Arrange
//            var reportData = Enumerable.Empty<WinlossByBettypeModel>();
//            var grandTotal = new WinlossByBettypeModel();
//            var expectedViewModel = new WinLossByBettypeViewModel(reportData, grandTotal, message);

//            // Act
//            var actualViewModel = handler.Handle(message);

//            // Act &  Assert
//            var result = compareLogic.Compare(expectedViewModel, actualViewModel);
//            Assert.True(result.AreEqual);
//        }
//    }
//}