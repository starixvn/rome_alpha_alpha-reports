namespace Alpha.Reports.Models
{
    public class ProductsByRefTable
    {
        public ProductsByRefTable()
        {

        }

        public ProductsByRefTable(int productId, byte refTable)
        {
            ProductId = productId;
            RefTable = refTable;
        }

        public int ProductId { get; }

        public byte RefTable { get; }
    }
}
