using System.Collections;

namespace Cardamom.Collections
{
    public class MultiMap<TKey, TValue> : IDictionary<TKey, IEnumerable<TValue>> where TKey : notnull
    {
        private readonly Dictionary<TKey, List<TValue>> _dict = new();

        public IEnumerable<TValue> this[TKey Key]
        {
            get => _dict.TryGetValue(Key, out var value) ? value : Enumerable.Empty<TValue>();
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
            if (_dict.TryGetValue(key, out var list))
            {
                list.Add(value);
            }
            else
            {
                _dict.Add(key, new List<TValue>() { value });
            }
        }

        public void Add(TKey key, IEnumerable<TValue> value)
        {
            if (_dict.TryGetValue(key, out var list))
            {
                list.AddRange(value);
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
            _dict.Clear();
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
            if (_dict.TryGetValue(key, out var list))
            {
                if (list.Remove(value))
                {
                    if (!list.Any())
                    {
                        Remove(key);
                    }
                    return true;
                }
            }
            return false;
        }

        public int RemoveAll(TKey key, Predicate<TValue> predicate)
        {
            if (_dict.TryGetValue(key, out var list))
            {
                return list.RemoveAll(predicate);
            }
            return 0;
        }

        public bool Remove(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out IEnumerable<TValue> value)
        {
            if (_dict.TryGetValue(key, out var list))
            {
                value = list;
                return true;
            }
            value = Enumerable.Empty<TValue>();
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}