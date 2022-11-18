namespace Cardamom.Collections
{
    public class Heap<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>> where TValue : IComparable
    {
        private KeyValuePair<TKey, TValue>[] _values;

        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public Heap()
        {
            _values = new KeyValuePair<TKey, TValue>[1];
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _values.Cast<KeyValuePair<TKey, TValue>>().Take(Count).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Push(keyValuePair);
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] destination, int arrayIndex)
        {
            Array.Copy(_values, 0, destination, arrayIndex, Count);
        }

        public void Push(TKey value, TValue priority)
        {
            Push(new KeyValuePair<TKey, TValue>(value, priority));
        }

        private void Push(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (Count == _values.Length)
            {
                var newValues = new KeyValuePair<TKey, TValue>[Count];
                Array.Copy(_values, newValues, Count);
                _values = newValues;
            }
            _values[Count] = keyValuePair;

            HeapifyUp(Count);
            Count++;
        }

        public TKey Peek() { return _values[0].Key; }

        public TKey Pop()
        {
            var value = _values[0].Key;
            Count--;
            if (Count == -1) Count = 0;
            _values[0] = _values[Count];
            if (Count == _values.Length / 4 && Count > 1)
            {
                var newValues = new KeyValuePair<TKey, TValue>[Count];
                Array.Copy(_values, newValues, Count);
                _values = newValues;
            }
            HeapifyDown(0);

            return value;
        }

        public bool Remove(TKey value)
        {
            int i;
            for (i = 0; i < Count; ++i)
            {
                if (_values[i].Key!.Equals(value))
                {
                    break;
                }
            }
            if (i < Count)
            {
                Count--;
                if (Count == -1)
                {
                    Count = 0;
                }
                _values[i] = _values[Count];
                if (Count == _values.Length / 4 && Count > 1)
                {
                    var newValues = new KeyValuePair<TKey, TValue>[Count];
                    Array.Copy(_values, newValues, Count);
                    _values = newValues;
                }
                HeapifyDown(i);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            throw new NotImplementedException();
        }

        private void Swap(int i1, int i2)
        {
            (_values[i2], _values[i1]) = (_values[i1], _values[i2]);
        }

        private void HeapifyDown(int index)
        {
            int c1 = index * 2 + 1;
            int c2 = index * 2 + 2;
            if (c1 >= Count && c2 >= Count)
            {
                return;
            }
            else if (c1 >= Count && c2 < Count)
            {
                if (_values[c2].Value.CompareTo(_values[index].Value) < 0)
                {
                    Swap(c2, index);
                    HeapifyDown(c2);
                }
            }
            else if (c1 < Count && c2 >= Count)
            {
                if (_values[c1].Value.CompareTo(_values[index].Value) < 0)
                {
                    Swap(c1, index);
                    HeapifyDown(c1);
                }
            }
            else
            {
                if (_values[c1].Value.CompareTo(_values[index].Value) < 0
                    && _values[c2].Value.CompareTo(_values[index].Value) >= 0)
                {
                    Swap(c1, index);
                    HeapifyDown(c1);
                }
                else if (_values[c1].Value.CompareTo(_values[index].Value) >= 0
                    && _values[c2].Value.CompareTo(_values[index].Value) < 0)
                {
                    Swap(c2, index);
                    HeapifyDown(c2);
                }
                else
                {
                    int i = _values[c1].Value.CompareTo(_values[c2].Value) < 0 ? c1 : c2;
                    Swap(i, index);
                    HeapifyDown(i);
                }
            }
        }

        private void HeapifyUp(int index)
        {
            int p = (index - 1) / 2;
            if (_values[index].Value.CompareTo(_values[p].Value) < 0)
            {
                Swap(index, p);
                HeapifyUp(p);
            }
        }
    }
}
