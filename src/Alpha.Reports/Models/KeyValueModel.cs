namespace Alpha.Reports.Models
{
    public class KeyValueModel
    {
        public KeyValueModel(object key, object value)
        {
            Key = key;
            Value = value;
        }

        public object Key { get; }

        public object Value { get; }
    }
}
