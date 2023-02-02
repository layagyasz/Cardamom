using OpenTK.Mathematics;

namespace Cardamom.Utils.Suppliers.Vector
{
    public class Vector4ComponentwiseSupplier : ISupplier<Vector4>
    {
        public ISupplier<float>? X { get; set; }
        public ISupplier<float>? Y { get; set; }
        public ISupplier<float>? Z { get; set; }
        public ISupplier<float>? W { get; set; }

        public Vector4 Get()
        {
            return new(X?.Get() ?? 0, Y?.Get() ?? 0, Z?.Get() ?? 0, W?.Get() ?? 0);
        }
    }
}
