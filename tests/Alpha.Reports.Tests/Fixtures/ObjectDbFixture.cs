namespace Alpha.Reports.Tests.Fixtures
{
    using Fanex.Data;
    using NSubstitute;
    using Reports.DataAccesses;

    public class ObjectDbFixture
    {
        public ObjectDbFixture()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.CreateInstance = (settingKey) => ObjectDb;
        }

        public IObjectDb ObjectDb { get; private set; }
    }
}
