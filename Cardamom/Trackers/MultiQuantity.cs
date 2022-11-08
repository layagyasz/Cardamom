namespace Cardamom.Trackers
{
    class MultiQuantity<T> : IDictionary<T, float> where T : notnull
    {
        private readonly Dictionary<T, float> _quantities = new();

        public ICollection<T> Keys => _quantities.Keys;
        public ICollection<float> Values => _quantities.Values;
        public int Count => _quantities.Count;
        public bool IsReadOnly => false;

        public float this[T key]
        {
            get { return Get(key); }
            set { Override(key, value); }
        }

        public IEnumerable<Quantity<T>> GetQuantities()
        {
            return this.Select(x => Quantity<T>.Create(x.Key, x.Value));
        }

        public float GetTotal()
        {
            return Values.Sum();
        }

        public void Add(T key, float amount)
        {
            if (_quantities.ContainsKey(key))
            {
                _quantities[key] += amount;
            }
            else
            {
                _quantities.Add(key, amount);
            }
        }

        public void Add(KeyValuePair<T, float> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        public void Add(MultiQuantity<T> counter)
        {
            foreach (var entry in counter)
            {
                Add(entry);
            }
        }

        public void Clear()
        {
            _quantities.Clear();
        }

        public bool Contains(KeyValuePair<T, float> keyValuePair)
        {
            return _quantities.Contains(keyValuePair);
        }

        public bool ContainsKey(T key)
        {
            return _quantities.ContainsKey(key);
        }

        public MultiQuantity<T> Copy()
        {
            MultiQuantity<T> newQuantity = new();
            foreach (var count in this)
            {
                newQuantity.Add(count);
            }
            return newQuantity;
        }

        public void CopyTo(KeyValuePair<T, float>[] keyValuePairs, int index)
        {
            throw new NotImplementedException();
        }

        public float Get(T Key)
        {
            bool found = _quantities.TryGetValue(Key, out float value);
            return found ? value : 0;
        }

        public IEnumerator<KeyValuePair<T, float>> GetEnumerator()
        {
            return _quantities.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Override(T key, float value)
        {
            if (_quantities.ContainsKey(key))
            {
                _quantities[key] = value;
            }
            else
            {
                _quantities.Add(key, value);
            }
        }

        public void Override(MultiQuantity<T> overrideQuantities)
        {
            foreach (var count in overrideQuantities)
            {
                Override(count.Key, count.Value);
            }
        }

        public bool Remove(T key)
        {
            return _quantities.Remove(key);
        }

        public bool Remove(KeyValuePair<T, float> keyValuePair)
        {
            return _quantities.Remove(keyValuePair.Key);
        }

        public bool TryGetValue(T key, out float value)
        {
            return _quantities.TryGetValue(key, out value);
        }

        public static MultiQuantity<T> operator *(float left, MultiQuantity<T> right)
        {
            MultiQuantity<T> newQuantity = new();
            foreach (var quantity in right)
            {
                newQuantity.Add(quantity.Key, left * quantity.Value);
            }
            return newQuantity;
        }
    }
}