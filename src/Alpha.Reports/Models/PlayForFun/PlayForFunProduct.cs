namespace Alpha.Reports.Models
{
    public class PlayForFunProduct : Product
    { 
        public PlayForFunProduct()
        {

        }

        public PlayForFunProduct(byte id, string name, bool isDisable) : base(id, name, null)
        {
            IsDisable = isDisable;
        }

        public bool IsDisable { get; }
    }
}