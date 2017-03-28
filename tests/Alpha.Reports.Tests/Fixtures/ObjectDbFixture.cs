namespace Alpha.Reports.Tests.Fixtures
{
    using Fanex.Data;
    using NSubstitute;

    public class ObjectDbFixture
    {
        public ObjectDbFixture()
        {
            ObjectDb = Substitute.For<IObjectDb>();
            Reports.DataAccesses.ObjectDbFactory.CreateInstance = (settingKey) => ObjectDb;
        }

        public IObjectDb ObjectDb { get; private set; }
    }
}
