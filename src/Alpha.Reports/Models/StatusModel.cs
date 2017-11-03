namespace Alpha.Reports.Models
{
    public class StatusModel
    {
        public StatusModel()
        {

        }

        public StatusModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }
}
