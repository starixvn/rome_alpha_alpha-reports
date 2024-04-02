using System.Collections.Generic;

namespace Alpha.Reports.Constants
{
    public static class ProductDetaiIdConstants
    {
        public const int Sportsbook = 11;
        public const int Soccer = 12;
        public const int Basketball = 13;
        public const int Cricket = 14;
        public const int Tennis = 20;
        public const int IceHockey = 23;

        public static readonly IEnumerable<int> SportsbookDetailFor777Site = new List<int>
            {
                Sportsbook,
                Soccer,
                Basketball,
                Cricket,
                Tennis,
                IceHockey
            };
    }
}