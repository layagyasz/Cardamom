using OpenTK.Mathematics;

namespace Cardamom.Planar
{
    public struct Transform2
    {
        public static readonly Transform2 Identity = new(Matrix3.Identity);

        private Matrix3 _matrix;

        public Transform2(Matrix3 matrix)
        {
            _matrix = matrix;
        }

        public Transform2 GetInverse()
        {
            return new(_matrix.Inverted());
        }

        public void Translate(Vector2 translation)
        {
            _matrix *=
                new Matrix3(
                    1, 0, 0,
                    0, 1, 0,
                    translation.X, translation.Y, 1);
        }

        public static Vector2 operator *(Transform2 left, Vector2 right)
        {
            return new(
                left._matrix.Row0.X * right.X + left._matrix.Row1.X * right.Y + left._matrix.Row2.X,
                left._matrix.Row0.Y * right.X + left._matrix.Row1.Y * right.Y + left._matrix.Row2.Y);
        }
    }
}
