namespace Alpha.Reports.Models
{
    public class League
    {
        public League()
        {
        }

        public League(int id, int sportTypeId, string name)
        {

        }

        internal League(int leagueid, int sporttype, string leaguename, byte leagueType)
        {
            LeagueId = leagueid;
            Name = leaguename;
            SportTypeId = sporttype;
            LeagueType = leagueType;
        }

        public string Id => $"{LeagueId}_{LeagueType}";

        public int LeagueId { get; }

        public string Name { get; }

        public int SportTypeId { get; }

        public string DisplayName => Name;

        public byte LeagueType { get; }
    }
}
