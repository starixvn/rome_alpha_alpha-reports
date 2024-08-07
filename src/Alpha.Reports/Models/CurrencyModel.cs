namespace Alpha.Reports.Models
{
    public class CurrencyModel
    {
        public CurrencyModel()
        {
        }

        public CurrencyModel(byte id, string name)
        {
            Id = id;
            Name = name;
        }

        public byte Id { get; set; }

        public string Name { get; set; }
    }
}
