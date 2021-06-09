namespace Alpha.Reports.Tests.Services
{
    using System.Collections.Generic;
    using Alpha.Reports.Criteria.RiskAnalysisIPMatching;
    using Alpha.Web.Helpers;
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
        private readonly PartialUMService partialUMService;
        private readonly RiskAnalysisIPMatchingServiceImpl service;

        public RiskAnalysisIPMatchingServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            repository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<PartialUMService>();
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
