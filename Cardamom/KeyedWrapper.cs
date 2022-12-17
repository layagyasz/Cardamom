namespace Cardamom
{
    public class KeyedWrapper<T> : IKeyed
    {
        public string Key { get; set; } = string.Empty;
        public T? Element { get; set; }
    }
}
