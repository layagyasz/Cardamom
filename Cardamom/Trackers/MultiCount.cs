namespace Cardamom.Trackers
{
    public class MultiCount<T> : IDictionary<T, int> where T : notnull
    {
        private readonly Dictionary<T, int> _counts = new();

        public ICollection<T> Keys => _counts.Keys;
        public ICollection<int> Values => _counts.Values;
        public int Count => _counts.Count;
        public bool IsReadOnly { get; } = false;

        public int this[T key]
        {
            get { return Get(key); }
            set { Override(key, value); }
        }

        public IEnumerable<Count<T>> GetCounts()
        {
            return this.Select(x => Count<T>.Create(x.Key, x.Value));
        }

        public int GetTotal()
        {
            return Values.Sum();
        }

        public void Add(T key, int amount)
        {
            if (_counts.ContainsKey(key))
            { 
                int newAmount = _counts[key] + amount;
                if (newAmount != 0)
                {
                    _counts[key] += amount;
                }
                else
                {
                    _counts.Remove(key);
                }
            }
            else
            {
                if (amount != 0)
                {
                    _counts.Add(key, amount);
                }
            }
        }

        public void Add(KeyValuePair<T, int> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        public void Add(MultiCount<T> counter)
        {
            foreach (var entry in counter)
            {
                Add(entry);
            }
        }

        public void Clear()
        {
            _counts.Clear();
        }

        public bool Contains(KeyValuePair<T, int> keyValuePair)
        {
            return _counts.Contains(keyValuePair);
        }

        public bool ContainsKey(T key)
        {
            return _counts.ContainsKey(key);
        }

        public MultiCount<T> Copy()
        {
            MultiCount<T> newCounter = new();
            foreach (var count in this)
            {
                newCounter.Add(count);
            }
            return newCounter;
        }

        public void CopyTo(KeyValuePair<T, int>[] keyValuePairs, int index)
        {
            throw new NotImplementedException();
        }

        public int Get(T key)
        {
            bool found = _counts.TryGetValue(key, out int value);
            return found ? value : 0;
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            return _counts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Override(T key, int value)
        {
            if (value == 0)
            {
                _counts.Remove(key);
            }
            else
            {
                if (_counts.ContainsKey(key))
                {
                    _counts[key] = value;
                }
                else
                {
                    _counts.Add(key, value);
                }
            }
        }

        public void Override(MultiCount<T> overrideCounters)
        {
            foreach (var count in overrideCounters)
            {
                Override(count.Key, count.Value);
            }
        }

        public bool Remove(T key)
        {
            return _counts.Remove(key);
        }

        public bool Remove(KeyValuePair<T, int> keyValuePair)
        {
            return _counts.Remove(keyValuePair.Key);
        }

        public bool TryGetValue(T key, out int value)
        {
            return _counts.TryGetValue(key, out value);
        }

        public static MultiCount<T> operator +(MultiCount<T> left, MultiCount<T> right)
        {
            MultiCount<T> newCounter = left.Copy();
            foreach (var count in right)
            {
                newCounter.Add(count);
            }
            return newCounter;
        }

        public static MultiCount<T> operator -(MultiCount<T> left, MultiCount<T> right)
        {
            MultiCount<T> newCounter = left.Copy();
            foreach (var count in right)
            {
                newCounter.Add(count.Key, -count.Value);
            }
            return newCounter;
        }

        public static MultiCount<T> operator *(int left, MultiCount<T> right)
        {
            MultiCount<T> newCounter = new();
            foreach (var count in right)
            {
                newCounter.Add(count.Key, left * count.Value);
            }
            return newCounter;
        }
    }
}