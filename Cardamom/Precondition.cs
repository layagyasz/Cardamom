namespace Cardamom
{
    public static class Precondition
    {
        public static void IsNull<T>(T? @object)
        {
            if (@object != null)
            {
                throw new ArgumentNullException(nameof(@object));
            }
        }

        public static T1 HasSize<T1, T2>(T1 @object, uint Size) where T1 : IEnumerable<T2>
        {
            if (@object.Count() != Size)
            {
                throw new ArgumentNullException(nameof(@object));
            }
            return @object;
        }

        public static T1 IsNotEmpty<T1, T2>(T1 @object) where T1 : IEnumerable<T2>
        {
            if (!@object.Any())
            {
                throw new ArgumentNullException(nameof(@object));
            }
            return @object;
        }
    }
}
