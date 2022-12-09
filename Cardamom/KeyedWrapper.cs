namespace Cardamom
{
    public class KeyedWrapper<T> : IKeyed
    {
        public string? Key { get; set; }
        public T? Element { get; set; }
    }
}
