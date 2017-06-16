namespace Alpha.Reports.Models
{
    public class CurrencyModel
    {
        public CurrencyModel(byte id, string name)
        {
            Id = id;
            Name = name;
        }

        public byte Id { get; }

        public string Name { get; }
    }
}
