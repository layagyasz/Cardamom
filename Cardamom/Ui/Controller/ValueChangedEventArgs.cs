namespace Cardamom.Ui.Controller
{
    public readonly struct ValueChangedEventArgs<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; }

        public ValueChangedEventArgs(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format($"[ValueChangedEventArgs: Key={Key}, Value={Value}]");
        }
    }
}
