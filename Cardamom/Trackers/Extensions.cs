namespace Cardamom.Trackers
{
    public static class Extensions
    {
        public static MultiCount<TKey> ToMultiCount<TKey, TIn>(
            this IEnumerable<TIn> source, Func<TIn, TKey> keySelector, Func<TIn, int> valueSelector)
            where TKey : notnull
        {
            var result = new MultiCount<TKey>();
            foreach (var value in source)
            {
                result.Add(keySelector(value), valueSelector(value));
            }
            return result;
        }

        public static MultiQuantity<TKey> ToMultiQuantity<TKey, TIn>(
            this IEnumerable<TIn> source, Func<TIn, TKey> keySelector, Func<TIn, float> valueSelector)
            where TKey : notnull
        {
            var result = new MultiQuantity<TKey>();
            foreach (var value in source)
            {
                result.Add(keySelector(value), valueSelector(value));
            }
            return result;
        }

        public static MultiCount<TKey> ToMultiCount<TKey>(this IEnumerable<TKey> source)
            where TKey : notnull
        {
            var result = new MultiCount<TKey>();
            foreach (var value in source.GroupBy(x => x))
            {
                result.Add(value.Key, value.Count());
            }
            return result;
        }
    }
}
