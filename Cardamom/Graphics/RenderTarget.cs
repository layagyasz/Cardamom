using Cardamom.Graphics.Core;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Cardamom.Graphics
{
    public abstract class RenderTarget : GraphicsContext
    {
        private IntRect _viewPort;
        private GLVertexArray? _vertexArray;

        protected RenderTarget(IntRect viewPort)
        {
            _viewPort = viewPort;
        }

        public abstract void SetActive(bool active);
        public abstract void Display();

        public void Clear()
        {
            SetActive(true);
            GL.Disable(EnableCap.ScissorTest);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public IntRect GetViewPort()
        {
            return _viewPort;
        }

        public void Draw(
            VertexArray vertices, int start, int count, Shader shader,Texture? texture)
        {
            Draw(vertices.Vertices, vertices.PrimitiveType, start, count, shader, texture);
        }

        public void Draw(
            Vertex2[] vertices, 
            PrimitiveType primitiveType,
            int start,
            int count,
            Shader shader, 
            Texture? texture)
        {
            _vertexArray ??= new(new());

            SetActive(true);
            Error.LogGLError("bind context");

            _vertexArray.SetData(vertices);

            if (texture != null)
            {
                texture?.Bind(TextureUnit.Texture0);
            }
            else
            {
                Texture.Unbind(TextureUnit.Texture0);
            }
            Error.LogGLError("bind context");

            shader.Bind();
            shader.SetMatrix3("projection", Transform2.CreateViewportOrthographicProjection(_viewPort).GetMatrix());
            shader.SetMatrix3("view", GetTransform().GetMatrix());

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFuncSeparate(
                BlendingFactorSrc.SrcAlpha,
                BlendingFactorDest.OneMinusSrcAlpha, 
                BlendingFactorSrc.One, 
                BlendingFactorDest.OneMinusSrcAlpha);
            Error.LogGLError("set blend");

            var scissor = GetScissor();
            if (scissor == null)
            {
                GL.Disable(EnableCap.ScissorTest);
            }
            else
            {
                if (scissor.Value.Size.X < 0 || scissor.Value.Size.Y < 0)
                {
                    return;
                }
                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor(
                    (int)scissor.Value.TopLeft.X, 
                    (int)(_viewPort.Size.Y - scissor.Value.TopLeft.Y - scissor.Value.Size.Y), 
                    (int)scissor.Value.Size.X,
                    (int)scissor.Value.Size.Y);
                Error.LogGLError($"set scissor {scissor}");
            }

            _vertexArray.Draw(primitiveType, start, count);

            SetActive(false);
        }

        public void Resize(Vector2i size)
        {
            _viewPort.Size = size;
        }
    }
}
