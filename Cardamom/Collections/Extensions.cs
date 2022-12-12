namespace Cardamom.Collections
{
    public static class Extensions
    {
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
