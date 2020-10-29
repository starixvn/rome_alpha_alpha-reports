namespace Alpha.Reports.Site.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Site.Controllers;
    using Site.Requests.Consolidation;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
    using Site.ViewModels.Consolidation;
    using Utils;
    using Xunit;

    [Trait("Category", "Consolidation")]
    [Collection("UserCollection")]
    public class ConsolidationControllerTests
    {
        private readonly IMediator mediator;
        private readonly ConsolidationController consolidationController;
        private readonly IUser user;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly CompareLogic compareLogic;

        public ConsolidationControllerTests(UserFixture userFixture)
        {
            user = userFixture.User;
            mediator = Substitute.For<IMediator>();
            consolidationController = new ConsolidationController(mediator, user);
            anyValueCreatorFixture = new AnyValueCreatorFixture();

            compareLogic = new CompareLogic();
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void HandleConsolidationByUsernameQuery_Always_CallsMediatorSendConsolidationByUsernameQuery()
        {
            // Arrange
            var message = new ConsolidationByUsernameQuery(user);

            // Act
            consolidationController.ConsolidationByUsername(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void HandleConsolidationByDate_Always_CallsMediatorSendConsolidationByDate()
        {
            // Arrange
            var message = new ConsolidationByDateQuery(user);

            // Act
            consolidationController.ConsolidationByDate(message);

            // Assert
            mediator.Received().Send<ConsolidationReportByDateViewModel>(message);
        }

        [Fact]
        public void HandleConsolidationByProductQuery_Always_CallsMediatorSendConsolidationByProductQuery()
        {
            // Arrange
            var message = new ConsolidationByProductQuery(user);

            // Act
            consolidationController.ConsolidationByProduct(message);

            // Assert
            mediator.Received().Send<ConsolidationReportByProductViewModel>(message);
        }

        [Fact]
        public void ConsolidationByUsernameExcel_Always_CallsMediatorSendConsolidationByUsernameExcelQuery()
        {
            // Arrange
            var message = new ConsolidationByUsernameExcelQuery(user);
            ControllerBaseFuctions.ArrangeExportExcel<ConsolidationByUsernameExcelQuery>(mediator);

            // Act
            consolidationController.ConsolidationByUsernameExcel(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void ConsolidationByDateExcel_Always_CallsMediatorSendConsolidationByDateExcelQuery()
        {
            // Arrange
            var message = new ConsolidationByDateExcelQuery(user);
            ControllerBaseFuctions.ArrangeExportExcel<ConsolidationByDateExcelQuery>(mediator);

            // Act
            consolidationController.ConsolidationByDateExcel(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void ConsolidationByProductExcel_Always_CallsMediatorSendConsolidationByProductExcelQuery()
        {
            // Arrange
            var message = new ConsolidationByProductExcelQuery(user);
            ControllerBaseFuctions.ArrangeExportExcel<ConsolidationByProductExcelQuery>(mediator);

            // Act
            consolidationController.ConsolidationByProductExcel(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void ConsolidationDetail_Always_CallsMediatorSendConsolidationDetailQuery()
        {
            // Arrange
            var message = new ConsolidationDetailQuery(user);

            // Act
            consolidationController.ConsolidationDetail(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}
