using OpenTK.Mathematics;

namespace Cardamom.Utils.Suppliers.Matrix
{
    public class Matrix4IdentitySupplier : ISupplier<Matrix4>
    {
        public Matrix4 Get()
        {
            return Matrix4.Identity;
        }
    }
}
