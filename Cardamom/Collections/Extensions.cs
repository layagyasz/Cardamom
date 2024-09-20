namespace Cardamom.Collections
{
    public static class Extensions
    {
        public static T? ArgMax<T>(this IEnumerable<T> source, Func<T, float> valueFn)
        {
            T? max = default;
            float value = float.MinValue;
            foreach (var item in source)
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

        public static T? ArgMin<T>(this IEnumerable<T> source, Func<T, float> valueFn)
        {
            T? min = default;
            float value = float.MaxValue;
            foreach (var item in source)
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

        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static EnumSet<T> ToEnumSet<T>(this IEnumerable<T> source)
            where T : Enum
        {
            var result = new EnumSet<T>();
            foreach (var value in source)
            {
                result.Add(value);
            }
            return result;
        }

        public static EnumMap<TKey, TValue> ToEnumMap<TKey, TValue, TIn>(
            this IEnumerable<TIn> source, Func<TIn, TKey> keySelector, Func<TIn, TValue> valueSelector)
            where TKey : Enum
        {
            var result = new EnumMap<TKey, TValue>();
            foreach (var value in source)
            {
                result.Add(keySelector(value), valueSelector(value));
            }
            return result;
        }

        public static Library<TOut> ToLibrary<TIn, TOut>(
            this IEnumerable<TIn> source, Func<TIn, string> keyFn, Func<TIn, TOut> valueFn)
        {
            var result = new Library<TOut>();
            foreach (var item in source)
            {
                result.Add(keyFn(item), valueFn(item));
            }
            return result;
        }
    }
}
