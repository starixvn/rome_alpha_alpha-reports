namespace Alpha.Reports.Tests.DataAccess
{
    using System.Collections.Generic;
    using Criteria;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Reports.DataAccess.Impl;

    public class ReportDataAccessImplTests
    {
        [Test]
        public void GetData_Always_ReturnsDataSourceGetByRepository()
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Create<List<dynamic>>();            
            var repository = Substitute.For<DynamicRepository>();
            repository
                .Get<List<dynamic>>(Arg.Any<ReportCriteriaBase>())
                .Returns(expected);
            var reportDataAccess = new ReportDataAccessImpl(repository);

            // Act
            var actual = reportDataAccess.GetData<dynamic>(new ReportCriteriaBase());

            // Assert
            Assert.AreEqual(expected, actual.DataSource);
        }
    }
}
