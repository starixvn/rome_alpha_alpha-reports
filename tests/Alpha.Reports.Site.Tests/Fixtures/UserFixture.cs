namespace Alpha.Reports.Site.Tests.Fixtures
{
    using NSubstitute;
    using Reports.Constants;
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
            User.GetHideProduct(ProductIdConstants.PokerGames).Returns(true);
        }

        public IUser User { get; private set; }
    }
}