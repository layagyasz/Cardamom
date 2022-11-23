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
            new("Shaders/default_planar_vert.sh", "Shaders/default_planar_frag.sh");

        public abstract IGLFWGraphicsContext GetContext();

        private GLVertexArray? _vertexArray;

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
            _vertexArray.Draw(vertices.PrimitiveType, start, count);
        }

        public void Resize(Vector2i size)
        {
            GetContext().MakeCurrent();
            GL.Viewport(0, 0, size.X, size.Y);
        }
    }
}
