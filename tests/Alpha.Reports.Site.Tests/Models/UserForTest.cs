namespace Alpha.Reports.Site.Tests.Models
{
    using System.Collections.Generic;
    using AlphaPlus.User.Models;
    using Site.Models;

    public class UserForTest : User
    {
        private readonly IDictionary<string, IList<int>> EnableProductsForGames = new Dictionary<string, IList<int>>
        {
            ["Jackpot"] = new List<int> { 6 }
        };

        public UserForTest(IUserSession userSession)
            : base(userSession)
        {
        }

        internal override IList<int> GetEnableProductsForGame(string gameName)
        => EnableProductsForGames[gameName];
    }
}