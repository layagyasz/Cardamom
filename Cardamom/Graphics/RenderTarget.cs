using Cardamom.Graphics.Core;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Cardamom.Graphics
{
    public abstract class RenderTarget
    {
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

        public void Draw(
            VertexArray vertices, int start, int count, Transform2 transform, Shader shader,Texture? texture)
        {
            Draw(vertices.Vertices, vertices.PrimitiveType, start, count, transform, shader, texture);
        }

        public void Draw(
            Vertex2[] vertices, 
            PrimitiveType primitiveType,
            int start,
            int count,
            Transform2 transform,
            Shader shader, 
            Texture? texture)
        {
            _vertexArray ??= new(new());

            GetContext().MakeCurrent();
            Error.LogGLError("bind context");

            _vertexArray.SetData(vertices);

            if (texture != null)
            {
                texture?.Bind(TextureUnit.Texture0);
            }
            else
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            shader.Bind();
            shader.SetMatrix3("projection", Transform2.CreateViewportOrthographicProjection(_viewPort).GetMatrix());
            shader.SetMatrix3("view", transform.GetMatrix());

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFuncSeparate(
                BlendingFactorSrc.SrcAlpha,
                BlendingFactorDest.OneMinusSrcAlpha, 
                BlendingFactorSrc.One, 
                BlendingFactorDest.OneMinusSrcAlpha);

            _vertexArray.Draw(primitiveType, start, count);
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
