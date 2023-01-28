using OpenTK.Mathematics;

namespace Cardamom.Utils.Suppliers.Vector
{
    public class Vector4UniformSupplier : ISupplier<Vector4>
    {
        public ISupplier<float>? ComponentValue { get; set; }

        public Vector4 Get()
        {
            var value = ComponentValue!.Get();
            return new(value, value, value, value);
        }
    }
}
