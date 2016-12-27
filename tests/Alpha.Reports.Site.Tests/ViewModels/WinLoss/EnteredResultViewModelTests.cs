namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Alpha.Reports.Models.Winloss;
    using Constants;
    using Enumerations;
    using Extensions;
    using Models;
    using Models.WinlossDetails;
    using NSubstitute;
    using Site.Requests.WinLoss;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels;
    using Site.ViewModels.WinLoss;
    using Site.ViewModels.WinLossDetails;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class EnteredResultViewModelTests
    {
        private readonly EnteredResultViewModel enteredResultViewModel;

        public EnteredResultViewModelTests()
        {
            enteredResultViewModel = new EnteredResultViewModel();
            
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Theory]
        [InlineData(4, "Master Win Loss")]
        [InlineData(3, "Agent Win Loss")]
        [InlineData(2, "Member Win Loss")]
        public void Title_EnteredResult_ReturnCorrectTitle(
            byte userLevel,
            string expectedTitle)
        {
            // Arrange
            enteredResultViewModel.UserLevel = Enumeration.FromValue<UserLevel>(userLevel);

            // Act & Assert
            Assert.Equal(enteredResultViewModel.Title, expectedTitle);
        }
    }
}
