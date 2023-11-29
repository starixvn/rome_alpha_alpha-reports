namespace Alpha.Reports.Tests.Services
{
    using System.Collections.Generic;
    using AlphaPlus.PartialUM.Services;
    using Alpha.Reports.Criteria.RiskAnalysisIPMatching;
    using Extensions;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using Reports.Models;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("AnyValueCreatorCollection")]
    public class RiskAnalysisIPMatchingServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly IDynamicRepository repository;
        private readonly IPartialUMService partialUMService;
        private readonly RiskAnalysisIPMatchingServiceImpl service;

        public RiskAnalysisIPMatchingServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            repository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<IPartialUMService>();
            service = new RiskAnalysisIPMatchingServiceImpl(repository, partialUMService);
        }

        [Fact]
        public void GetRiskAnalysisIPMatchingModel_Always_ReturnsCorrectDataFromRepository()
        {
            // Arrange
            var param = anyValueCreatorFixture.Create<RiskAnalysisIPMatchingParam>();
            var data = new List<RiskAnalysisIPMatchingModel> { anyValueCreatorFixture.Create<RiskAnalysisIPMatchingModel>() };
            repository.Fetch<RiskAnalysisIPMatchingModel>(Arg.Any<RiskAnalysisIPMatchingCriteria>())
                .Returns(data);

            repository.Fetch<RiskAnalysisIPMatchingModel>(Arg.Any<RiskAnalysisIPMatchingCriteria>()).Returns(data);

            var expectedData = data;

            // Act
            var actualData = service.GetRiskAnalysisIPMatchingModel(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }
    }
}