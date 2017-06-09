namespace Alpha.Reports.Site.Tests.Fixtures
{
    using NSubstitute;
    using Reports.Models;

    public class UserFixture
    {
        public UserFixture()
        {
            User = Substitute.For<IUser>();
            User.LevelId.Returns((byte)4);
            User.Id.Returns(10);
            User.Name.Returns("Name");
            User.Language.Returns("en-US");
            User.CurrencyName.Returns("USS");
        }

        public IUser User { get; private set; }
    }
}