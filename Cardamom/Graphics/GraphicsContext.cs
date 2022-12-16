using Cardamom.Geometry;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public abstract class GraphicsContext
    {
        private readonly Stack<Matrix4> _transformStack = new();
        private readonly Stack<FloatRect?> _scissorStack = new();

        public FloatRect? GetScissor()
        {
            return _scissorStack.Count == 0 ? null : _scissorStack.Peek();
        }

        public Matrix4 GetTransform()
        {
            return _transformStack.Count == 0 ? Matrix4.Identity : _transformStack.Peek();
        }

        public void PopScissor()
        {
            _scissorStack.Pop();
        }

        public void PushEmptyScissor()
        {
            _scissorStack.Push(null);
        }

        public void PushScissor(FloatCube scissor)
        {
            var currentScissor = GetScissor();
            var transformed = Combine(scissor, GetTransform());
            _scissorStack.Push(
                currentScissor == null ? transformed : currentScissor.Value.GetIntersection(transformed));
        }

        public void PopTransform()
        {
            _transformStack.Pop();
        }

        public void PushTransform(Matrix4 transform)
        {
            _transformStack.Push(transform * GetTransform());
        }

        public void PushTranslation(Vector3 translation)
        {
            PushTransform(Matrix4.CreateTranslation(translation));
        }

        private static FloatRect Combine(FloatCube rect, Matrix4 transform)
        {
            var topLeft = new Vector4(rect.FrontTopLeft.X, rect.FrontTopLeft.Y, rect.FrontTopLeft.Z, 1f) * transform;
            var bottomRight =
                new Vector4(
                    rect.FrontTopLeft.X + rect.Size.X,
                    rect.FrontTopLeft.Y + rect.Size.Y,
                    rect.FrontTopLeft.Z + rect.Size.Z,
                    1f)
                * transform;

            var top = Math.Min(topLeft.Y, bottomRight.Y);
            var left = Math.Min(topLeft.X, bottomRight.X);
            var bottom = Math.Max(topLeft.Y, bottomRight.Y);
            var right = Math.Max(topLeft.X, bottomRight.X);

            return new(new(left, top), new(right - left, bottom - top));
        }
    }
}
