using OpenTK.Mathematics;

namespace Cardamom.Utils.Suppliers.Matrix
{
    public class Matrix4DiagonalUniformSupplier : ISupplier<Matrix4>
    {
        public ISupplier<float>? Diagonal { get; set; }

        public Matrix4 Get()
        {
            var diagonal = Diagonal!.Get();
            return new(
                new(diagonal, 0, 0, 0),
                new(0, diagonal, 0, 0),
                new(0, 0, diagonal, 0),
                new(0, 0, 0, diagonal));
        }
    }
}
