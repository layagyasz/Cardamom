namespace Cardamom.Collections
{
    public class WeightedVector<T> : ICollection<KeyValuePair<T, double>>
    {
        double[] _values;
        T[] _keys;
        int _Alloc;

        public double Total { get; private set; }
        public int Count { get; private set; }
        public bool IsReadOnly { get; } = false;

        public WeightedVector()
        {
            Count = 0;
            _Alloc = 1;
            _values = new double[1];
            _keys = new T[1];
        }

        public WeightedVector(WeightedVector<T> Copy)
        {
            Count = Copy.Count;
            _Alloc = Copy._Alloc;
            _values = new double[_Alloc];
            _keys = new T[_Alloc];
            Total = Copy.Total;
            for (int i = 0; i < _Alloc; i++)
            {
                _keys[i] = Copy._keys[i];
                _values[i] = Copy._values[i];
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

        public void Add(double Weight, T Value)
        {
            if (Count + 1 == _Alloc)
            {
                _Alloc *= 2;
                T[] newK = new T[_Alloc];
                double[] newA = new double[_Alloc];
                Array.Copy(_keys, newK, _keys.Length);
                Array.Copy(_values, newA, _keys.Length);
                _values = newA;
                _keys = newK;
            }
            Total += Weight;
            _keys[Count] = Value;
            _values[Count + 1] = _values[Count] + Weight;
            ++Count;
        }

        public void Add(KeyValuePair<T, double> Value)
        {
            Add(Value.Value, Value.Key);
        }

        private int IndexOf(double V)
        {
            if (Count == 0) throw new Exception("Index on Empty WeightVector<T>");
            if (Count == 1) return 0;
            int i = 0;
            int j = Count - 1;
            int c = (j + i) / 2;
            while (j - i > 1)
            {
                if (V > _values[c])
                {
                    i = c;
                    c = (j + i) / 2;
                }
                else if (V < _values[c])
                {
                    j = c;
                    c = (j + i) / 2;
                }
                else break;
            }
            if (j - i == 1 && _values[c] > V) c--;
            else if (j - i == 1 && _values[c + 1] < V) c++;
            return c;
        }

        public T this[double V]
        {
            get
            {
                return _keys[IndexOf(V * Total)];
            }
            set
            {
                _keys[IndexOf(V * Total)] = value;
            }
        }

        public void Clear()
        {
            Count = 0;
            _Alloc = 1;
            _values = new double[1];
            _keys = new T[1];
        }

        public bool Contains(KeyValuePair<T, double> Value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<T, double> Value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<T, double>[] Values, int Index)
        {
            throw new NotImplementedException();
        }
    }
}