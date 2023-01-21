using Cardamom.Collections;

namespace Cardamom.Graphing
{
    public static class StableMatching
    {
        public static IEnumerable<Tuple<TLeft, TRight>> Compute<TLeft, TRight>(
            IEnumerable<TLeft> left,
            IEnumerable<TRight> right, 
            Func<TLeft, TRight, float> leftPreferenceFn, 
            Func<TLeft, TRight, float> rightPreferenceFn)
            where TLeft : notnull 
            where TRight : notnull
        {
            (var leftWrappers, var rightWrappers) = 
                BipartiteGraph.Generate(
                    left, right, new GraphGenerator<TLeft, TRight>(leftPreferenceFn, rightPreferenceFn));

            Stack<IBipartiteNode> open = new(leftWrappers);
            while (open.Count > 0)
            {
                var current = (LeftWrapper)open.Pop();
                while (current.HasNextPreference())
                {
                    while (Equals(current.Match, default))
                    {
                        var r = (RightWrapper)current.GetNextPreference();
                        var preference = r.GetCost(current);
                        var currentMatch = (LeftWrapper?)r.Match;
                        if (Equals(currentMatch, default))
                        {
                            r.Update(current);
                            current.Update(r);
                        }
                        else if (r.GetCost(current) > r.GetCost(currentMatch))
                        {
                            open.Push(currentMatch);
                            currentMatch.Update(default);
                            r.Update(current);
                            current.Update(r);
                        }
                    }
                }
            }

            return leftWrappers.Select(x => new Tuple<TLeft, TRight>((TLeft)x.Value, (TRight)x.Match!.Value));
        }

        private class GraphGenerator<TLeft, TRight> 
            : IBipartiteGraphGenerator<TLeft, LeftWrapper, TRight, RightWrapper> 
            where TLeft : notnull 
            where TRight : notnull
        {
            private readonly Func<TLeft, TRight, float> _leftPreferenceFn;
            private readonly Func<TLeft, TRight, float> _rightPreferenceFn;

            public GraphGenerator(
                Func<TLeft, TRight, float> leftPreferenceFn, Func<TLeft, TRight, float> rightPreferenceFn)
            {
                _leftPreferenceFn = leftPreferenceFn;
                _rightPreferenceFn = rightPreferenceFn;
            }

            public LeftWrapper GenerateNode(int id, TLeft value, int numNeighbors)
            {
                return new LeftWrapper(id, value, numNeighbors);
            }

            public RightWrapper GenerateNode(int id, TRight value, int numNeighbors)
            {
                return new RightWrapper(id, value, numNeighbors);
            }

            public float GetLefthandCost(TLeft left, TRight right)
            {
                return _leftPreferenceFn(left, right);
            }

            public float GetRighthandCost(TLeft left, TRight right)
            {
                return _rightPreferenceFn(left, right);
            }
        }

        private class LeftWrapper : IBipartiteNode
        {
            public int Id { get; }
            public object Value { get; }
            public IBipartiteNode? Match { get; private set; }
            private readonly Heap<IBipartiteNode, float> _preferences;

            public LeftWrapper(int id, object value, int numNeighbors)
            {
                Id = id;
                Value = value;
                _preferences = new(numNeighbors);
            }

            public float GetCost(IBipartiteNode other)
            {
                throw new NotImplementedException();
            }

            public void SetCost(IBipartiteNode other, float preference)
            {
                _preferences.Push(other, preference);
            }

            public IBipartiteNode GetNextPreference()
            {
                return _preferences.Pop();
            }

            public bool HasNextPreference()
            {
                return _preferences.Count > 0;
            }

            public void Update(IBipartiteNode? match)
            {
                Match = match;
            }
        }

        private class RightWrapper : IBipartiteNode
        {
            public int Id { get; }
            public object Value { get; }
            public IBipartiteNode? Match { get; private set; }
            private readonly float[] _preferences;

            public RightWrapper(int id, object value, int numNeighbors)
            {
                Id = id;
                Value = value;
                _preferences = new float[numNeighbors];
            }

            public void SetCost(IBipartiteNode other, float preference)
            {
                _preferences[other.Id] = preference;
            }

            public float GetCost(IBipartiteNode other)
            {
                return _preferences[other.Id];
            }

            public void Update(IBipartiteNode match)
            {
                Match = match;
            }
        }
    }
}
