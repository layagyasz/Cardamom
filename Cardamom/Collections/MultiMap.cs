using System.Collections;

namespace Cardamom.Collections
{
    public class MultiMap<TKey, TValue> : IDictionary<TKey, IEnumerable<TValue>> where TKey : notnull
    {
        private readonly Dictionary<TKey, List<TValue>> _dict = new();

        public IEnumerable<TValue> this[TKey Key]
        {
            get => _dict.ContainsKey(Key) ? _dict[Key] : Enumerable.Empty<TValue>();
            set => _dict[Key] = value.ToList();
        }

        public ICollection<TKey> Keys => _dict.Keys;

        public ICollection<IEnumerable<TValue>> Values => _dict.Values.Select(x => x.AsEnumerable()).ToList();

        public int Count => _dict.Count;
        public bool IsReadOnly => false;

        public MultiMap() { }

        public MultiMap(IDictionary<TKey, IEnumerable<TValue>> copy)
        {
            foreach (var entry in copy)
            {
                Add(entry);
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (_dict.ContainsKey(key))
            {
                _dict[key].Add(value);
            }
            else
            {
                _dict.Add(key, new List<TValue>() { value });
            }
        }

        public void Add(TKey key, IEnumerable<TValue> value)
        {
            if (_dict.ContainsKey(key))
            {
                _dict[key].AddRange(value);
            }
            else
            {
                _dict.Add(key, value.ToList());
            }
        }

        public void Add(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, IEnumerable<TValue>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> GetEnumerator()
        {
            return _dict.Select(
                x => new KeyValuePair<TKey, IEnumerable<TValue>>(x.Key, x.Value)).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }

        public bool Remove(TKey key, TValue value)
        {
            if (_dict.ContainsKey(key))
            {
                return _dict[key].Remove(value);
            }
            return false;
        }

        public int RemoveAll(TKey Key, Predicate<TValue> predicate)
        {
            if (_dict.ContainsKey(Key))
            {
                return _dict[Key].RemoveAll(predicate);
            }
            return 0;
        }

        public bool Remove(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out IEnumerable<TValue> value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}