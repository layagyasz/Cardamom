﻿using Cardamom.Graphics.Core;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Cardamom.Graphics
{
    public abstract class RenderTarget : GraphicsContext
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
            GL.Disable(EnableCap.ScissorTest);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Display()
        {
            GetContext().SwapBuffers();
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
                    (int)(_viewPort.Bottom - scissor.Value.TopLeft.Y - scissor.Value.Size.Y), 
                    (int)scissor.Value.Size.X,
                    (int)scissor.Value.Size.Y);
                Error.LogGLError($"set scissor {scissor}");
            }

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
