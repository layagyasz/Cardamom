namespace Cardamom.Collections
{
    public class WeightedVector<T> : IDictionary<T, float>
    {
        private float[] _values;
        private T[] _keys;

        public float Total { get; private set; }
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public float this[T key]
        {
            get => throw new NotImplementedException();
            set
            {
                if (ContainsKey(key))
                {
                    throw new ArgumentException("Key already exists.");
                }
                Add(key, value);
            }
        }

        public ICollection<T> Keys
        {
            get => _keys.Take(Count).ToList();
        }

        public ICollection<float> Values
        {
            get => _values.Take(Count).ToList();
        }

        public WeightedVector()
        {
            Count = 0;
            _values = new float[1];
            _keys = new T[1];
        }

        public WeightedVector(WeightedVector<T> copy)
        {
            Count = copy.Count;
            _values = new float[copy._values.Length];
            _keys = new T[copy._keys.Length];
            Total = copy.Total;
            Array.Copy(copy._values, _values, copy.Count);
            Array.Copy(copy._keys, _keys, copy.Count);
        }

        public IEnumerator<KeyValuePair<T, float>> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return new KeyValuePair<T, float>(_keys[i], _values[i]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T key, float value)
        {
            if (Count + 1 == _values.Length)
            {
                T[] newK = new T[2 * _values.Length];
                float[] newA = new float[2 * _values.Length];
                Array.Copy(_keys, newK, _keys.Length);
                Array.Copy(_values, newA, _keys.Length);
                _values = newA;
                _keys = newK;
            }
            Total += value;
            _keys[Count] = key;
            _values[Count + 1] = _values[Count] + value;
            ++Count;
        }

        public void Add(KeyValuePair<T, float> value)
        {
            Add(value.Key, value.Value);
        }

        public T Get(float x)
        {
            return _keys[IndexOf(x)];
        }

        public T Get(int index)
        {
            return _keys[index];
        }

        public bool ContainsKey(T key)
        {
            return Array.IndexOf(_keys, key, 0, Count) >= 0;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(KeyValuePair<T, float> value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T, float>[] values, int index)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<T, float> value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(T key, out float value)
        {
            int index = Array.IndexOf(_keys, key, 0, Count);
            if (index < 0)
            {
                value = default;
                return false;
            }
            value = _values[index];
            return true;
        }

        private int IndexOf(double interval)
        {
            if (Count == 0)
            {
                throw new Exception("Index on Empty WeightVector<T>");
            }
            if (Count == 1)
            {
                return 0;
            }
            int i = 0;
            int j = Count - 1;
            int c = (j + i) / 2;
            while (j - i > 1)
            {
                if (interval > _values[c])
                {
                    i = c;
                    c = (j + i) / 2;
                }
                else if (interval < _values[c])
                {
                    j = c;
                    c = (j + i) / 2;
                }
                else break;
            }
            if (j - i == 1 && _values[c] > interval)
            {
                c--;
            }
            else if (j - i == 1 && _values[c + 1] < interval)
            {
                c++;
            }
            return c;
        }
    }
}