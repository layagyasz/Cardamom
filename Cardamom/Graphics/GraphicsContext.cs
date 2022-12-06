using Cardamom.Planar;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public abstract class GraphicsContext
    {
        private readonly Stack<Transform2> _transformStack = new();
        private readonly Stack<FloatRect> _scissorStack = new();

        public FloatRect? GetScissor()
        {
            return _scissorStack.Count == 0 ? null : _scissorStack.Peek();
        }

        public Transform2 GetTransform()
        {
            return _transformStack.Count == 0 ? Transform2.Identity : _transformStack.Peek();
        }

        public void PopScissor()
        {
            _scissorStack.Pop();
        }

        public void PushScissor(FloatRect scissor)
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

        public void PushTransform(Transform2 transform)
        {
            _transformStack.Push(transform * GetTransform());
        }

        public void PushTranslation(Vector2 translation)
        {
            PushTransform(Transform2.CreateTranslation(translation));
        }

        private static FloatRect Combine(FloatRect rect, Transform2 transform)
        {
            var topLeft = transform * rect.TopLeft;
            var bottomRight = transform * (rect.TopLeft + rect.Size);

            var top = Math.Min(topLeft.Y, bottomRight.Y);
            var left = Math.Min(topLeft.X, bottomRight.X);
            var bottom = Math.Max(topLeft.Y, bottomRight.Y);
            var right = Math.Max(topLeft.X, bottomRight.X);

            return new(new(left, top), new(right - left, bottom - top));
        }
    }
}
