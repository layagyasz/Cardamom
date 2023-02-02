using OpenTK.Mathematics;

namespace Cardamom.Utils.Suppliers.Matrix
{
    public class Matrix4DiagonalVectorSupplier : ISupplier<Matrix4>
    {
        public ISupplier<Vector4>? Diagonal { get; set; }

        public Matrix4 Get()
        {
            var diagonal = Diagonal?.Get() ?? new();
            return new(
                new(diagonal.X, 0, 0, 0),
                new(0, diagonal.Y, 0, 0),
                new(0, 0, diagonal.Z, 0),
                new(0, 0, 0, diagonal.W));
        }
    }
}
