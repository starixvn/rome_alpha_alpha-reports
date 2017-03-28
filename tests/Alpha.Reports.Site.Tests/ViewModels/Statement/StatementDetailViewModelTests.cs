using System;
using System.Collections.Generic;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Site.Tests.Constants;
using Alpha.Reports.Site.Tests.Extensions;
using Alpha.Reports.Site.Tests.Fixtures;
using Alpha.Reports.Site.ViewModels.Statement;
using Alpha.Reports.Utils;
using Xunit;

namespace Alpha.Reports.Site.Tests.ViewModels.Statement
{
    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class StatementDetailViewModelTests
    {
        private readonly IUser user;

        public StatementDetailViewModelTests(UserFixture userFixture)
        {
            this.user = userFixture.User;
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void Total_Always_ReturnsCorrectValue()
        {
            // Arrange
            var query = new StatementDetailsQuery(user);
            var statements = new List<StatementDetailModel>
            {
               new StatementDetailModel(DefaultVariables.CurrentDate, string.Empty, string.Empty, 10, string.Empty, string.Empty),
               new StatementDetailModel(DefaultVariables.CurrentDate, string.Empty, string.Empty, 20, string.Empty, string.Empty)
            };

            var viewModel = new StatementDetailViewModel(query, statements);
            var expectedTotal = new StatementDetailModel(DateTime.MinValue, string.Empty, string.Empty, 30, string.Empty, string.Empty);

            // Act
            var actualTotal = viewModel.Total;

            // Assert
            AssertExtension.Equal(expectedTotal, actualTotal);
        }
    }
}
