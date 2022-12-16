using Cardamom.Geometry;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public abstract class GraphicsContext
    {
        private IntRect _viewPort;
        private Matrix4 _defaultProjection;
        private readonly Stack<Matrix4> _projectionStack = new();
        private readonly Stack<Matrix4> _viewStack = new();
        private readonly Stack<FloatRect?> _scissorStack = new();

        protected GraphicsContext(IntRect viewPort) 
        {
            SetViewPort(viewPort);
        }

        public abstract void Clear();
        public abstract void Flatten();

        public Matrix4 GetProjectionMatrix()
        {
            return _projectionStack.Count == 0 ? _defaultProjection : _projectionStack.Peek();
        }

        public FloatRect? GetScissor()
        {
            return _scissorStack.Count == 0 ? null : _scissorStack.Peek();
        }

        public Matrix4 GetViewMatrix()
        {
            return _viewStack.Count == 0 ? Matrix4.Identity : _viewStack.Peek();
        }

        public IntRect GetViewPort()
        {
            return _viewPort;
        }

        public void PopScissor()
        {
            _scissorStack.Pop();
        }

        public void PushEmptyScissor()
        {
            _scissorStack.Push(null);
        }

        public void PushProjectionMatrix(Matrix4 projection)
        {
            _projectionStack.Push(projection);
        }

        public void PushScissor(FloatCube scissor)
        {
            var currentScissor = GetScissor();
            var transformed = Combine(scissor, GetViewMatrix());
            _scissorStack.Push(
                currentScissor == null ? transformed : currentScissor.Value.GetIntersection(transformed));
        }

        public void PopProjectionMatrix()
        {
            _projectionStack.Pop();
        }

        public void PopViewMatrix()
        {
            _viewStack.Pop();
        }

        public void PushViewMatrix(Matrix4 transform)
        {
            _viewStack.Push(transform * GetViewMatrix());
        }

        public void PushTranslation(Vector3 translation)
        {
            PushViewMatrix(Matrix4.CreateTranslation(translation));
        }

        public void SetViewPort(IntRect viewPort)
        {
            _viewPort = viewPort;
            _defaultProjection = 
                Matrix4.CreateOrthographicOffCenter(0, _viewPort.Size.X, _viewPort.Size.Y, 0, -10000, 10000);
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
