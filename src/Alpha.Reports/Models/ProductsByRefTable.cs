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

        public int ProductId { get; set; }

        public byte RefTable { get; set; }
    }
}
