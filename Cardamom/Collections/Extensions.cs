namespace Cardamom.Collections
{
    public static class Extensions
    {
        public static T? ArgMax<T>(this IEnumerable<T> items, Func<T, float> valueFn)
        {
            T? max = default;
            float value = float.MinValue;
            foreach (var item in items)
            {
                float v = valueFn(item);
                if (max == null || value < v)
                {
                    max = item;
                    value = v;
                }
            }
            return max;
        }

        public static T? ArgMin<T>(this IEnumerable<T> items, Func<T, float> valueFn)
        {
            T? min = default;
            float value = float.MaxValue;
            foreach (var item in items)
            {
                float v = valueFn(item);
                if (min == null || value > v)
                {
                    min = item;
                    value = v;
                }
            }
            return min;
        }

        public static Library<TOut> ToLibrary<TIn, TOut>(
            this IEnumerable<TIn> items, Func<TIn, string> keyFn, Func<TIn, TOut> valueFn)
        {
            var result = new Library<TOut>();
            foreach (var item in items)
            {
                result.Add(keyFn(item), valueFn(item));
            }
            return result;
        }
    }
}
