namespace Cardamom.Collections
{
    public class EnumMap<TKey, TValue> : IDictionary<TKey, TValue> where TKey : struct, IConvertible
    {
        readonly TValue?[] _values;

        public TValue this[TKey key]
        {
            get => Precondition.NotNull(_values[(int)(object)key]);
            set => _values[(int)(object)key] = value;
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return Enum.GetValues(typeof(TKey)).Cast<TKey>().Where(this.ContainsKey).ToList();
            }
        }
        public ICollection<TValue> Values { 
            get { return _values.Where(x => !x?.Equals(default) ?? false).Select(Precondition.NotNull).ToList(); }
        }
        public int Count { get { return _values.Length; } }
        public bool IsReadOnly { get; } = false;

        public EnumMap()
        {
            if (!typeof(TKey).IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum.", typeof(TKey).Name));
            }
            _values = new TValue[Enum.GetValues(typeof(TKey)).Length];
        }

        public EnumMap(EnumMap<TKey, TValue> copy) : this()
        {
            _values = copy._values.ToArray();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (TKey key in Enum.GetValues(typeof(TKey)))
            {
                if (!this[key]?.Equals(default) ?? false)
                {
                    yield return new KeyValuePair<TKey, TValue>(key, Precondition.NotNull(this[key]));
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            for (int i = 0; i < _values.Length; ++i)
            {
                _values[i] = default;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return !this[key]?.Equals(default(TValue)) ?? false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> value)
        {
            return this[value.Key]?.Equals(value.Value) ?? false;
        }

        public void Add(TKey key, TValue value)
        {
            this[key] = value;
        }

        public void Add(KeyValuePair<TKey, TValue> value)
        {
            this[value.Key] = value.Value;
        }

        public bool Remove(TKey key)
        {
            _values[(int)(object)key] = default;
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> value)
        {
            if (Contains(value))
            {
                return Remove(value.Key);
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = this[key];
            return value?.Equals(default) ?? false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] values, int index)
        {
            int i = 0;
            foreach (TKey Key in Keys)
            {
                values[i + index] = new KeyValuePair<TKey, TValue>(Key, Precondition.NotNull(this[Key]));
            }
        }
    }
}