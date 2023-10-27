namespace Cardamom.Collections
{
    public class ArrayList<T> : ICollection<T>
    {
        public int Count { get; private set; }
        public bool IsReadOnly => false;

        private T[] _values;

        public T this[int index]
        {
            get
            {
                if (index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return _values[index];
            }
            set
            {
                if (index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }
                _values[index] = value;
            }
        }

        public ArrayList()
        {
            _values = new T[1];
        }

        public ArrayList(int initialSize)
        {
            _values = new T[initialSize];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _values.Take(Count).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (Count == _values.Length)
            {
                Resize();
            }
            _values[Count++] = item;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            var index = Array.IndexOf(_values, item);
            return index >= 0 && index < Count;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_values, 0, array, arrayIndex, Count);
        }

        public T[] GetData()
        {
            return _values;
        }

        public void Trim(int length)
        {
            Count -= length;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        private void Resize()
        {
            var newValues = new T[2 * _values.Length];
            Array.Copy(_values, newValues, _values.Length);
            _values = newValues;
        }
    }
}
