namespace Alpha.Reports.Site.Tests.Fixtures
{
    using NSubstitute;
    using Reports.Models;

    public class UserFixture
    {
        public UserFixture()
        {
            User = Substitute.For<IUser>();
        }

        public IUser User { get; private set; }
    }
}
