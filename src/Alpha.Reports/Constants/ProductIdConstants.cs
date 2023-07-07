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
        public const int Allbet = 21;
        public const int SAGaming = 39;
        public const int SexyGaming = 45;
        public const int GTCasino = 46;
        public const int WM = 51;
        public const int ION = 52;
        public const int BigGame = 57;
        public const int BBin = 47;
        public const int PP = 32;
        public const int GDG = 59;
        public const int PPLiveCasino = 65;

        public const int ArcadiaGaming = 34;
        public const int SG = 36;
        public const int PGSoft = 55;
        public const int Joker = 56;
        public const int MaxGame = 60;
        public const int Habanero = 61;
        public const int CG = 62;
        public const int VGaming = 66;
        public const int AdvantPlay = 67;
        public const int AdvantPlayMini = 68;
        public const int RNGWar = 50;
        public const int SabaClub = 42;
        public const int DummyRNGProductId = 53;
        public const int FGG = 70;
        public const int YeeBet = 76;

        public const int Cashout = 25;

        public static readonly IEnumerable<int> ProductListForTooltipsLiveCasino = new List<int>
            {
                Allbet,
                SexyGaming,
                SAGaming,
                GTCasino,
                BBin,
                ION,
                WM,
                GDG,
                BigGame,
                PPLiveCasino,
                YeeBet,
                FGG
            };

        public static readonly IEnumerable<int> ProductListForTooltipsRNGCasino = new List<int>
            {
                RNGWar,
                SabaClub,
                VGaming
            };
    }
}