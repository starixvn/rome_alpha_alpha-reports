using System.Collections.Generic;

namespace Alpha.Reports.Constants
{
    public static class ProductIdConstants
    {
        public const int Sportsbook = 1;
        public const int NumberGame = 3;
        public const int VirtualSports = 5;
        public const int RNGCasino = 6;
        public const int MacauGames = 22;
        public const int FishingWorld = 24;
        public const int NG2 = 23;
        public const int Allbet = 21;
        public const int SAGaming = 39;
        public const int SexyGaming = 45;
        public const int GTCasino = 46;
        public const int WM = 51;
        public const int ION = 52;
        public const int BBin = 47;
        public const int PP = 32;
        public const int GDG = 59;
        public static readonly List<int> ProductListForTooltipsLiveCasino = new List<int>
            {
                ProductIdConstants.Allbet,
                ProductIdConstants.SexyGaming,
                ProductIdConstants.SAGaming,
                ProductIdConstants.GTCasino,
                ProductIdConstants.BBin,
                ProductIdConstants.ION,
                ProductIdConstants.WM,
                ProductIdConstants.GDG
            };
    }
}