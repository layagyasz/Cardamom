using Cardamom.Mathematics;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public abstract class GraphicsContext
    {
        private Box2i _viewPort;
        private readonly Stack<Projection> _projectionStack = new();
        private readonly Stack<Matrix4> _viewStack = new();
        private readonly Stack<Matrix4> _modelStack = new();
        private readonly Stack<Box2?> _scissorStack = new();

        protected GraphicsContext(Box2i viewPort) 
        {
            SetViewPort(viewPort);
        }

        public abstract void Clear();
        public abstract void Clear(Color4 color);
        public abstract void Flatten();

        public Matrix4 GetModelMatrix()
        {
            return _modelStack.Peek();
        }

        public Projection GetProjection()
        {
            return _projectionStack.Peek();
        }

        public Box2? GetScissor()
        {
            return _scissorStack.Count == 0 ? null : _scissorStack.Peek();
        }

        public Matrix4 GetViewMatrix()
        {
            return _viewStack.Peek();
        }

        public Box2i GetViewPort()
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

        public void PushProjection(Projection projection)
        {
            _projectionStack.Push(projection);
        }

        public void PushScissor(Box3 scissor)
        {
            var currentScissor = GetScissor();
            var transformed = Combine(scissor, GetModelMatrix() * GetViewMatrix());
            _scissorStack.Push(
                currentScissor == null ? transformed : currentScissor.Value.GetIntersection(transformed));
        }

        public void PopModelMatrix()
        {
            _modelStack.Pop();
        }

        public void PopProjectionMatrix()
        {
            _projectionStack.Pop();
        }

        public void PopViewMatrix()
        {
            _viewStack.Pop();
        }

        public void PushModelMatrix(Matrix4 transform)
        {
            if (_modelStack.Count == 0)
            {
                _modelStack.Push(transform);
            }
            else
            {
                _modelStack.Push(transform * GetModelMatrix());
            }
        }

        public void PushTranslation(Vector3 translation)
        {
            PushModelMatrix(Matrix4.CreateTranslation(translation));
        }

        public void PushViewMatrix(Matrix4 view)
        {
            _viewStack.Push(view);
        }

        public void SetViewPort(Box2i viewPort)
        {
            _viewPort = viewPort;
        }

        private static Box2 Combine(Box3 rect, Matrix4 transform)
        {
            var topLeft = new Vector4(rect.Min.X, rect.Min.Y, rect.Min.Z, 1f) * transform;
            var bottomRight = new Vector4(rect.Max.X, rect.Max.Y, rect.Max.Z, 1f) * transform;

            var top = Math.Min(topLeft.Y, bottomRight.Y);
            var left = Math.Min(topLeft.X, bottomRight.X);
            var bottom = Math.Max(topLeft.Y, bottomRight.Y);
            var right = Math.Max(topLeft.X, bottomRight.X);

            return new(new(left, top), new(right, bottom));
        }
    }
}
