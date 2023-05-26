namespace Cardamom.Utils
{
    public class FluentComparator<T> : IComparer<T>
    {
        readonly List<Func<T, IComparable>> _keys = new();
        bool _invert;

        private FluentComparator() { }

        public static FluentComparator<T> Comparing(Func<T, IComparable> key)
        {
            return new FluentComparator<T>().Then(key);
        }

        public FluentComparator<T> Then(Func<T, IComparable> Key)
        {
            _keys.Add(Key);
            return this;
        }

        public FluentComparator<T> Invert()
        {
            _invert = true;
            return this;
        }

        public int Compare(T? left, T? right)
        {
            if (left == null && right == null)
            {
                return 0;
            }
            if (left != null && right == null)
            {
                return 1;
            }
            if (left == null && right != null)
            {
                return -1;
            }
            foreach (var key in _keys)
            {
                int result = key(left!).CompareTo(key(right!));
                if (result != 0)
                {
                    return (_invert ? -1 : 1) * result;
                }
            }
            return 0;
        }
    }
}