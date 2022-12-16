﻿using Cardamom.Geometry;
using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public abstract class RenderTarget : GraphicsContext
    {
        private GLVertexArray? _vertexArray;

        protected RenderTarget(IntRect viewPort)
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
            VertexArray vertices, int start, int count, Shader shader,Texture? texture)
        {
            Draw(vertices.Vertices, vertices.PrimitiveType, start, count, shader, texture);
        }

        public void Draw(
            Vertex3[] vertices, 
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
            shader.SetMatrix4("projection", GetProjectionMatrix());
            shader.SetMatrix4("view", GetViewMatrix());

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
                    (int)scissor.Value.TopLeft.X, 
                    (int)(GetViewPort().Size.Y - scissor.Value.TopLeft.Y - scissor.Value.Size.Y), 
                    (int)scissor.Value.Size.X,
                    (int)scissor.Value.Size.Y);
                Error.LogGLError($"set scissor {scissor}");
            }

            _vertexArray.Draw(primitiveType, start, count);

            SetActive(false);
        }
    }
}
