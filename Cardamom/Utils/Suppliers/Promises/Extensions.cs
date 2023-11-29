namespace Cardamom.Utils.Suppliers.Promises
{
    public static class Extensions
    {
        public static IPromise<TOut> Map<TIn, TOut>(this IPromise<TIn> parent,  Func<TIn, TOut> mapper)
        {
            return new MapPromise<TIn, TOut>(parent, mapper);
        }
    }
}
