﻿using OpenTK.Mathematics;

namespace Cardamom.Geometry
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

        public Matrix3 GetMatrix()
        {
            return _matrix;
        }

        public static Transform2 CreateTranslation(Vector2 translation)
        {
            return new(
                new Matrix3(
                    1, 0, 0,
                    0, 1, 0,
                    translation.X, translation.Y, 1));
        }

        public static Transform2 CreateViewportOrthographicProjection(IntRect viewPort)
        {
            return CreateViewportOrthographicProjection(
                viewPort.TopLeft.X, 
                viewPort.TopLeft.X + viewPort.Size.X,
                viewPort.TopLeft.Y,
                viewPort.TopLeft.Y + viewPort.Size.Y);
        }

        public static Transform2 CreateViewportOrthographicProjection(float left, float right, float top, float bottom)
        {
            var result = Identity;

            var invRL = 1.0f / (right - left);
            var invTB = 1.0f / (top - bottom);

            result._matrix.Row0.X = 2 * invRL;
            result._matrix.Row1.Y = 2 * invTB;

            result._matrix.Row2.X = -(right + left) * invRL;
            result._matrix.Row2.Y = -(top + bottom) * invTB;

            return result;
        }

        public static Transform2 operator *(Transform2 left, Transform2 right)
        {
            return new(left._matrix * right._matrix);
        }

        public static Vector2 operator *(Transform2 left, Vector2 right)
        {
            return new(
                left._matrix.Row0.X * right.X + left._matrix.Row1.X * right.Y + left._matrix.Row2.X,
                left._matrix.Row0.Y * right.X + left._matrix.Row1.Y * right.Y + left._matrix.Row2.Y);
        }
    }
}