using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Cardamom
{
    public static class Precondition
    {
        [return: NotNull]
        public static T NotNull<T>(T? @object)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }
            return @object;
        }

        public static void IsNull<T>(T? @object)
        {
            if (@object != null)
            {
                throw new ArgumentNullException(nameof(@object));
            }
        }

        public static T HasSize<T>(T @object, uint Size) where T : ICollection
        {
            if (@object.Count != Size)
            {
                throw new ArgumentNullException(nameof(@object));
            }
            return @object;
        }
    }
}
