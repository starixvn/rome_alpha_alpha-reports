namespace Alpha.Reports.Tests.DataAccesses
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Alpha.Reports.Criteria.Statement;
    using Alpha.Reports.DataAccesses;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Tests.Constants;
    using Alpha.Reports.Tests.Database.Statement;
    using Alpha.Reports.Tests.Extensions;
    using Alpha.Reports.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("ObjectDbCollection")]
    public class StatementDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly StatementDataAccessImpl statementDataAccessImpl;
        private readonly CompareLogic compareLogic;

        public StatementDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            statementDataAccessImpl = new StatementDataAccessImpl();
            compareLogic = new CompareLogic();
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void GetData_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new GetStatementCriteria(0, SystemTime.Today, SystemTime.Today, SystemTime.Today, string.Empty);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataReader);

            var expectedData = new List<StatementModel>();

            // Act
            var actualData = statementDataAccessImpl.GetStatements(criteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetData_HasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new GetStatementCriteria(0, SystemTime.Today, SystemTime.Today, SystemTime.Today, string.Empty);

            var dataSet = StatementDatabase.StatementData.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<Hashtable>()).Returns(dataSet);

            var expectedData = new List<StatementModel>
            {
                new StatementModel(new System.DateTime(2017, 5, 19), 1, 0, 1, 2, 3, 4, 5, 6, 7, 8),
                new StatementModel(new System.DateTime(2017, 5, 19), 11, null, null, null, null, null, null, null, null, null)
            };

            // Act
            var actualData = statementDataAccessImpl.GetStatements(criteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }
    }
}
