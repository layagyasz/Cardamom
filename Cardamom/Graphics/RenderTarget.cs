using Cardamom.Graphics.Core;
using Cardamom.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public abstract class RenderTarget : GraphicsContext
    {
        private VertexBuffer<Vertex3>? _defaultBuffer;

        protected RenderTarget(Box2i viewPort)
            : base(viewPort) { }

        public abstract void SetActive(bool active);
        public abstract void Display();

        public override void Clear()
        {
            SetActive(true);
            GL.Disable(EnableCap.ScissorTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public override void Flatten()
        {
            SetActive(true);
            GL.Disable(EnableCap.ScissorTest);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public void Draw(
            VertexArray vertices, int start, int count, RenderResources resources)
        {
            Draw(vertices.Vertices, vertices.PrimitiveType, start, count, resources);
        }

        public void Draw(
            Vertex3[] vertices, PrimitiveType primitiveType, int start, int count, RenderResources resources)
        {
            _defaultBuffer ??= new(new());
            _defaultBuffer.Buffer(vertices);
            _defaultBuffer.PrimitiveType = primitiveType;
            Draw(_defaultBuffer, start, count, resources);
        }

        public void Draw<T>(VertexBuffer<T> buffer, int start, int count, RenderResources resources) 
            where T :struct
        {
            SetActive(true);
            Error.LogGLError("bind context");

            if (resources.Texture0 != null)
            {
                resources.Texture0.Bind(TextureUnit.Texture0);
            }
            if (resources.Texture1 != null)
            {
                resources.Texture1.Bind(TextureUnit.Texture1);
            }

            Error.LogGLError("bind context");

            resources.Shader.Bind();
            resources.Shader.SetMatrix4("projection", GetProjection().Matrix);
            resources.Shader.SetMatrix4("view", GetViewMatrix());
            resources.Shader.SetMatrix4("model", GetModelMatrix());

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

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
                    (int)scissor.Value.Min.X, 
                    (int)(GetViewPort().Size.Y - scissor.Value.Min.Y - scissor.Value.Size.Y), 
                    (int)scissor.Value.Size.X,
                    (int)scissor.Value.Size.Y);
                Error.LogGLError($"set scissor {scissor}");
            }

            buffer.Draw(start, count);

            if (resources.Texture0 != null)
            {
                Texture.Unbind(TextureUnit.Texture0);
            }
            if (resources.Texture1 != null)
            {
                Texture.Unbind(TextureUnit.Texture1);
            }
            SetActive(false);
        }
    }
}
