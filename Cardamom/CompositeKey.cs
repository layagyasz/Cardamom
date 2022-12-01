namespace Cardamom
{
    public struct CompositeKey<T1, T2>
    {
        public T1 Key1 { get; set; }
        public T2 Key2 { get; set; }
        
        public CompositeKey(T1 key1, T2 key2)
        {
            Key1 = key1;   
            Key2 = key2;
        }

        public static CompositeKey<T1, T2> Create(T1 key1, T2 key2)
        {
            return new CompositeKey<T1, T2>(key1, key2);
        }
    }
}
