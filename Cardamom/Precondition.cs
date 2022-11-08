using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom
{
    public static class Precondition
    {
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
