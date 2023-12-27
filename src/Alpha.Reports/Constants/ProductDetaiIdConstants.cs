using System.Collections.Generic;

namespace Alpha.Reports.Constants
{
    public static class ProductDetaiIdConstants
    {
        public const int Soccer = 12;
        public const int Basketball = 13;
        public const int Cricket = 14;
        public const int Tennis = 20;

        public static readonly IEnumerable<int> SportsbookDetailFor777Site = new List<int>
            {
                Soccer,
                Basketball,
                Cricket,
                Tennis
            };
    }
}