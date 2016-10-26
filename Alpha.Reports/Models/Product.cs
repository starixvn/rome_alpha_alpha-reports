namespace Alpha.Reports.Models
{
    public class Product
    {
        public Product(            
            int id,
            string name,
            string permission,
            bool hasNetTurnoverToolTip)
        {            
            Id = id;
            Name = name;
            Permission = permission;
            HasNetTurnoverToolTip = hasNetTurnoverToolTip;
        }        

        public int Id { get; }

        public string Name { get; }

        public string Permission { get; }

        public bool HasNetTurnoverToolTip { get; }
    }
}
