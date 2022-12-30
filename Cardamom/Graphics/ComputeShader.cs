using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class ComputeShader : GLShader
    {
        private ComputeShader(int handle)
            : base(handle) { }

        public static ComputeShader FromFile(string path)
        {
            int handle = CompileShader(path, ShaderType.ComputeShader);
            int program = GL.CreateProgram();
            GL.AttachShader(program, handle);
            LinkProgram(program);
            GL.DetachShader(program, handle);
            GL.DeleteShader(handle);
            return new ComputeShader(program);
        }

        public void DoCompute(Vector2i size)
        {
            Bind();
            GL.DispatchCompute(size.X, size.Y, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
        }
    }
}
