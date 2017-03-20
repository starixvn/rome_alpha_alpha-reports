namespace Alpha.Reports.Models
{
    public class PlayForFunProduct : Product
    {
        public PlayForFunProduct(byte id, string name, bool isDisable) : base(id, name)
        {
            IsDisable = isDisable;
        }

        public bool IsDisable { get; }
    }
}