using System;

namespace Alpha.Reports.Models
{
    public struct Product
    {
        public Product(int id, Func<int, string> getProductName)
        {            
            Id = id;
            Name = getProductName(id);
        }        

        public int Id { get; }

        public string Name { get; }
    }
}
