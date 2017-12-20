namespace Alpha.Reports.Models
{
    public class ProductsByRefTable
    {
        public ProductsByRefTable()
        {

        }

        public ProductsByRefTable(byte productId, byte refTable)
        {
            ProductId = productId;
            RefTable = refTable;
        }

        public byte ProductId { get; }

        public byte RefTable { get; }
    }
}
