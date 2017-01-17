namespace Alpha.Reports.Models
{
    using System;

    public class League
    {
        public League(int id, string name, int sportTypeId)
        {
            Id = id;
            Name = name;
            SportTypeId = sportTypeId;
        }

        internal League(int id, string name, int sportTypeId, string displayName) : this(id, name, sportTypeId)
        {
            DisplayName = displayName;
        }

        public int Id { get; }

        public string Name { get; }

        public int SportTypeId { get; }

        public string DisplayName { get; }
    }
}
