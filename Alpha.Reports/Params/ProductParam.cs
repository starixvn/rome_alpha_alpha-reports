namespace Alpha.Reports.Params
{
    public class ProductParam
    {
        public ProductParam(string name, int id, bool isSelected)
        {
            Name = name;
            Id = id;
            IsSelected = isSelected;
        }

        public string Name { get; }

        public int Id { get; }

        public bool IsSelected { get; }
    }
}
