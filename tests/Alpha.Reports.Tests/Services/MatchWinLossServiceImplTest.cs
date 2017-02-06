namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using Reports.Criteria.MatchWinLoss;
    using Reports.Models.MatchWinLoss;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossServiceImplTest
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly MatchWinLossServiceImpl matchWinLossServiceImpl;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MatchWinLossServiceImplTest(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            matchWinLossServiceImpl = new MatchWinLossServiceImpl(dynamicRepository);

            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetMatchWinLoss_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossServiceImpl.GetMatchWinLoss(nullParam));
        }

        [Fact]
        public void GetMatchWinLoss_ValidParam_CallsDynamicRepository()
        {
            // Arrange
            var param = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate);

            // Act
            matchWinLossServiceImpl.GetMatchWinLoss(param);

            // Assert
            dynamicRepository.Received().Fetch<MatchWinLossModel>(Arg.Is<GetMatchWinLossCriteria>(
                criteria => criteria.CustId == param.CustId
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate));
        }

        [Fact]
        public void GetMatchWinLoss_ValidParam_ReturnsDataFromDynamicRepository()
        {
            var param = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate);

            var expectedMatchWinLossModels = new List<MatchWinLossModel>
            {
                 anyValueCreatorFixture.Create<MatchWinLossModel>(),
                 anyValueCreatorFixture.Create<MatchWinLossModel>(),
                 anyValueCreatorFixture.Create<MatchWinLossModel>()
            };

            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<GetMatchWinLossCriteria>()).Returns(expectedMatchWinLossModels);

            // Act
            var actualMatchWinLossModels = matchWinLossServiceImpl.GetMatchWinLoss(param);

            // Assert
            AssertExtension.Equal(expectedMatchWinLossModels, actualMatchWinLossModels);
        }
    }
}
