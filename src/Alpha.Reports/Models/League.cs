namespace Alpha.Reports.Models
{
    public class League
    {
        public League()
        {
        }

        public League(int id, int sportTypeId, string name)
        {
            Id = id;
            Name = name;
            SportTypeId = sportTypeId;
            DisplayName = name;
        }

        internal League(int id, string name, int sportTypeId, string displayName) : this(id, sportTypeId, name)
        {
            DisplayName = displayName;
        }

        public int Id { get; }

        public string Name { get; }

        public int SportTypeId { get; }

        public string DisplayName { get; }
    }
}
