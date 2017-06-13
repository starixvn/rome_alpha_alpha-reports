using System.Collections.Generic;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Params.Statement;
using Alpha.Reports.Services;
using Alpha.Reports.Site.Handlers.Statement;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Site.Tests.Extensions;
using Alpha.Reports.Site.Tests.Fixtures;
using Alpha.Reports.Site.ViewModels.Statement;
using NSubstitute;
using Xunit;

namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class StatementHandlerTests
    {
        private readonly IUser user;
        private readonly StatementService reportService;
        private readonly StatementHandler handler;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public StatementHandlerTests(UserFixture userFixture)
        {
            user = userFixture.User;
            reportService = Substitute.For<StatementService>();
            handler = new StatementHandler(reportService);

            anyValueCreatorFixture = new AnyValueCreatorFixture();
        }

        [Fact]
        public void HandleSMAStatementQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var query = new SMAStatementQuery(user);
            var data = new List<StatementModel>
            {
                anyValueCreatorFixture.Create<StatementModel>(),
                anyValueCreatorFixture.Create<StatementModel>(),
                anyValueCreatorFixture.Create<StatementModel>()
            };

            reportService.GetStatements(Arg.Any<GetStatementParam>()).Returns(data);
            var expectedViewModel = new SMAStatementViewModel(query, data);

            // Act
            var actualViewModel = handler.Handle(query);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleStatementDetailsQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var query = new StatementDetailsQuery(user);
            var data = new List<StatementDetailModel>
            {
                anyValueCreatorFixture.Create<StatementDetailModel>(),
                anyValueCreatorFixture.Create<StatementDetailModel>(),
                anyValueCreatorFixture.Create<StatementDetailModel>()
            };

            reportService.StatementDetails(Arg.Any<StatementDetailsParam>()).Returns(data);
            var expectedViewModel = new StatementDetailViewModel(query, data);

            // Act
            var actualViewModel = handler.Handle(query);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleMemberStatementQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var data = new List<StatementModel>
            {
                anyValueCreatorFixture.Create<StatementModel>(),
                anyValueCreatorFixture.Create<StatementModel>(),
                anyValueCreatorFixture.Create<StatementModel>()
            };

            reportService.GetStatements(Arg.Any<GetStatementParam>()).Returns(data);
            var expectedViewModel = new MemberStatementViewModel(query, data);

            // Act
            var actualViewModel = handler.Handle(query);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }
    }
}
