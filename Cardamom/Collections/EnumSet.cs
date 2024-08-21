namespace Cardamom.Collections
{
    public class EnumSet<T> : ISet<T> where T : Enum
    {
        readonly bool[] _values;

        public int Count { get => _values.Count(x => x); }
        public bool IsReadOnly { get; } = false;

        public EnumSet()
        {
            _values = new bool[Enum.GetValues(typeof(T)).Cast<T>().Max(x => (int)(object)x) + 1];
        }

        public EnumSet(IEnumerable<T> items)
            : this()
        {
            UnionWith(items);
        }

        public EnumSet(params T[] items)
            : this(items.ToList()) { }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (Contains(item))
                {
                    yield return item;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Add(T item)
        {
            if (Contains(item))
            {
                return false;
            }
            _values[(int)(object)item] = true;
            return true;
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public void Clear()
        {
            for (int i=0;i<_values.Length;++i)
            {
                _values[i] = false;
            }
        }

        public bool Contains(T item)
        {
            return _values[(int)(object)item];
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                Remove(item);
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            var otherSet = new EnumSet<T>(other);
            for (int i=0; i<_values.Length; ++i)
            {
                _values[i] &= otherSet._values[i];
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return IsSubsetOf(other) && Count != other.Count();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return IsSupersetOf(other) && Count != other.Count();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            foreach (var item in this)
            {
                if (!other.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return other.Any(Contains);
        }

        public bool Remove(T item)
        {
            if (!Contains(item))
            {
                return false;
            }
            _values[(int)(object)item] = false;
            return true;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return IsSupersetOf(other) && Count == other.Count();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                _values[(int)(object)item] ^= true;
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                Add(item);
            }
        }

        public override string ToString()
        {
            return string.Format($"[{string.Join(", ", this)}]");
        }
    }
}