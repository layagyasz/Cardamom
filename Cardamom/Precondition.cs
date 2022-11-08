using System.Diagnostics.CodeAnalysis;

namespace Cardamom
{
    public static class Precondition
    {
        [return: NotNull]
        public static T CheckNotNull<T>(T? @object)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }
            return @object;
        }
    }
}
