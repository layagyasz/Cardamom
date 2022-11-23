using Cardamom.Graphics.Core;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Cardamom.Graphics
{
    public abstract class RenderTarget
    {
        private static readonly Shader _shader =
            new Shader.Builder()
                .SetVertex("Shaders/default_planar_vert.sh")
                .SetFragment("Shaders/default_planar_frag.sh")
                .Build();

        private ViewPort _viewPort;
        private GLVertexArray? _vertexArray;

        public abstract IGLFWGraphicsContext GetContext();

        protected RenderTarget(ViewPort viewPort)
        {
            _viewPort = viewPort;
        }

        public void Clear()
        {
            GetContext().MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Display()
        {
            GetContext().SwapBuffers();
        }

        public void Draw(VertexArray vertices, int start, int count, Transform2 transform)
        {
            _vertexArray ??= new(new());

            GetContext().MakeCurrent();
            Error.LogGLError("bind context");

            _vertexArray.SetData(vertices.Vertices);
            _shader.Bind();
            _shader.SetMatrix3("projection", Transform2.CreateViewportOrthographicProjection(_viewPort).GetMatrix());
            _shader.SetMatrix3("view", transform.GetMatrix());
            _vertexArray.Draw(vertices.PrimitiveType, start, count);
        }

        public void Resize(Vector2i size)
        {
            GetContext().MakeCurrent();
            GL.Viewport(0, 0, size.X, size.Y);
            _viewPort.Right = size.X;
            _viewPort.Bottom = size.Y;
        }
    }
}
