using Cardamom.Collections;

namespace Cardamom.Graphing
{
    public static class StableMatching
    {
        public static IEnumerable<Tuple<TLeft, TRight>> Compute<TLeft, TRight>(
            IEnumerable<TLeft> left, IEnumerable<TRight> right) where TLeft : IGraphNode where TRight : IGraphNode
        {
            Dictionary<TLeft, LeftWrapper<TLeft, TRight>> leftWrappers = 
                left.Select(LeftWrapper<TLeft, TRight>.Wrap).ToDictionary(x => x.Key, x => x);
            Dictionary<TRight, RightWrapper<TLeft, TRight>> rightWrappers = 
                right.Select(RightWrapper<TLeft, TRight>.Wrap).ToDictionary(x => x.Key, x => x);
            foreach (var l in leftWrappers.Values)
            {
                foreach (var r in l.Key.GetEdges())
                {
                    l.AddPreference(rightWrappers[(TRight)r.End], r.Cost);
                }
            }
            foreach (var r in rightWrappers.Values)
            {
                foreach (var l in  r.Key.GetEdges())
                {
                    r.AddPreference(leftWrappers[(TLeft)l.End], l.Cost);
                }
            }

            Stack<LeftWrapper<TLeft, TRight>> open = new(leftWrappers.Values);
            while (open.Count > 0)
            {
                var current = open.Pop();
                while (current.HasNextPreference())
                {
                    while (Equals(current.Match, default))
                    {
                        var r = current.GetNextPreference();
                        var preference = r.GetPreference(current);
                        if (preference > r.CurrentPreference)
                        {
                            if (!Equals(r.Match, default))
                            {
                                open.Push(r.Match);
                                r.Match.Update(default);
                            }
                            r.Update(current, preference);
                            current.Update(r);
                        }
                    }
                }
            }

            return leftWrappers.Values.Select(x => new Tuple<TLeft, TRight>(x.Key, x.Match!.Key));
        }

        private class LeftWrapper<T, K>
        {
            public T Key { get; }
            public RightWrapper<T, K>? Match { get; private set; }
            private readonly Heap<RightWrapper<T, K>, float> _preferences = new();

            private LeftWrapper(T key)
            {
                Key = key;
            }

            public static LeftWrapper<T, K> Wrap(T left)
            {
                return new LeftWrapper<T, K>(left);
            }

            public void AddPreference(RightWrapper<T, K> rightWrapper, float preference)
            {
                _preferences.Push(rightWrapper, preference);
            }

            public RightWrapper<T, K> GetNextPreference()
            {
                return _preferences.Pop();
            }

            public bool HasNextPreference()
            {
                return _preferences.Count > 0;
            }

            public void Update(RightWrapper<T, K>? match)
            {
                Match = match;
            }
        }

        private class RightWrapper<T, K>
        {
            public K Key { get; }
            public LeftWrapper<T, K>? Match { get; private set; }
            public float CurrentPreference { get; private set; } = float.PositiveInfinity;
            private readonly Dictionary<LeftWrapper<T, K>, float> _preferences = new();

            private RightWrapper(K key)
            {
                Key = key;
            }

            public static RightWrapper<T, K> Wrap(K right)
            {
                return new RightWrapper<T, K>(right);
            }

            public void AddPreference(LeftWrapper<T, K> leftWrapper, float preference)
            {
                _preferences.Add(leftWrapper, preference);
            }

            public float GetPreference(LeftWrapper<T, K> leftWrapper)
            {
                return _preferences[leftWrapper];
            }

            public void Update(LeftWrapper<T, K> match, float preference)
            {
                Match = match;
                CurrentPreference = preference;
            }
        }
    }
}
