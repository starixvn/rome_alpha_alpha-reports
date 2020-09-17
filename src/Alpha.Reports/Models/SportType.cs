namespace Alpha.Reports.Models
{
    using System;

    public class SportType
    {
        public SportType()
        {
        }

        public SportType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        internal SportType(int id, string name, Func<int, string> getSportTypeDisplayName)
            : this(id, name)
        {
            DisplayName = getSportTypeDisplayName == null ? name : getSportTypeDisplayName(id);
        }

        public int Id { get; }

        public string Name { get; }

        public string DisplayName { get; }
    }
}
