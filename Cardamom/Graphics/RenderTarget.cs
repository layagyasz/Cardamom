using Cardamom.Graphics.Core;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;

namespace Cardamom.Graphics
{
    public abstract class RenderTarget
    {
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
            _vertexArray.SetData(vertices.Vertices);
            _vertexArray.Draw(vertices.PrimitiveType, start, count);
        }
    }
}
