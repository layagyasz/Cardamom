using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class ComputeShader : GLShader
    {
        private Vector2 _localGroupSize;

        private ComputeShader(int handle, Vector2i localGroupSize)
            : base(handle) 
        { 
            _localGroupSize = new(1f / localGroupSize.X, 1f / localGroupSize.Y);
        }

        public static ComputeShader FromFile(string path, Vector2i localGroupSize)
        {
            int handle = CompileShader(path, ShaderType.ComputeShader);
            int program = GL.CreateProgram();
            GL.AttachShader(program, handle);
            LinkProgram(program);
            GL.DetachShader(program, handle);
            GL.DeleteShader(handle);
            return new ComputeShader(program, localGroupSize);
        }

        public void DoCompute(Vector2i size)
        {
            Bind();
            GL.DispatchCompute(
                (int)Math.Ceiling(size.X * _localGroupSize.X), (int)Math.Ceiling(size.Y * _localGroupSize.Y), 1);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
        }
    }
}
