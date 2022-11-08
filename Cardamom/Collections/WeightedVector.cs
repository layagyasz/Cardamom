namespace Cardamom.Collections
{
    public class WeightedVector<T> : ICollection<KeyValuePair<T, double>>
    {
        double[] _values;
        T[] _keys;
        int _alloc;

        public double Total { get; private set; }
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public WeightedVector()
        {
            Count = 0;
            _alloc = 1;
            _values = new double[1];
            _keys = new T[1];
        }

        public WeightedVector(WeightedVector<T> copy)
        {
            Count = copy.Count;
            _alloc = copy._alloc;
            _values = new double[_alloc];
            _keys = new T[_alloc];
            Total = copy.Total;
            for (int i = 0; i < _alloc; i++)
            {
                _keys[i] = copy._keys[i];
                _values[i] = copy._values[i];
            }
        }

        public IEnumerator<KeyValuePair<T, double>> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return new KeyValuePair<T, double>(_keys[i], _values[i]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(double weight, T value)
        {
            if (Count + 1 == _alloc)
            {
                _alloc *= 2;
                T[] newK = new T[_alloc];
                double[] newA = new double[_alloc];
                Array.Copy(_keys, newK, _keys.Length);
                Array.Copy(_values, newA, _keys.Length);
                _values = newA;
                _keys = newK;
            }
            Total += weight;
            _keys[Count] = value;
            _values[Count + 1] = _values[Count] + weight;
            ++Count;
        }

        public void Add(KeyValuePair<T, double> value)
        {
            Add(value.Value, value.Key);
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

        public T this[double interval]
        {
            get
            {
                return _keys[IndexOf(interval * Total)];
            }
            set
            {
                _keys[IndexOf(interval * Total)] = value;
            }
        }

        public void Clear()
        {
            Count = 0;
            _alloc = 1;
            _values = new double[1];
            _keys = new T[1];
        }

        public bool Contains(KeyValuePair<T, double> value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<T, double> value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T, double>[] values, int index)
        {
            throw new NotImplementedException();
        }
    }
}