using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Core
{
    public class GLShader : GLObject
    {
        protected GLShader(int handle)
            : base(handle) { }

        public void Bind()
        {
            GL.UseProgram(Handle);
            Error.LogGLError("bind shader");
        }

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(Handle, name);
        }

        public int[] GetUniformIndices(string[] names)
        {
            var indices = new int[names.Length];
            GL.GetUniformIndices(Handle, names.Length, names, indices);
            return indices;
        }

        public int[] GetUniformOffsets(int[] indices)
        {
            var offsets = new int[indices.Length];
            GL.GetActiveUniforms(Handle, indices.Length, indices, ActiveUniformParameter.UniformOffset, offsets);
            return offsets;
        }

        public int[] GetUniformOffsets(string[] names)
        {
            return GetUniformOffsets(GetUniformIndices(names));
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(Handle, name);
        }

        public int GetUniformBlockIndex(string name)
        {
            return GL.GetUniformBlockIndex(Handle, name);
        }

        public int GetUniformBlockSize(string name)
        {
            return GetUniformBlockSize(GetUniformBlockIndex(name));
        }

        public int GetUniformBlockSize(int index)
        {
            GL.GetActiveUniformBlock(Handle, index, ActiveUniformBlockParameter.UniformBlockDataSize, out int size);
            return size;
        }

        public void SetInt32(string name, int data)
        {
            Bind();
            GL.Uniform1(GetUniformLocation(name), data);
        }

        public void SetInt32(int location, int data)
        {
            Bind();
            GL.Uniform1(location, data);
        }

        public void SetInt32Array(string name, int[] data)
        {
            Bind();
            GL.Uniform1(GetUniformLocation(name), data.Length, data);
        }

        public void SetInt32Array(int location, int[] data)
        {
            Bind();
            GL.Uniform1(location, data.Length, data);
        }

        public void SetFloat(string name, float data)
        {
            Bind();
            GL.Uniform1(GetUniformLocation(name), data);
        }

        public void SetFloat(int location, float data)
        {
            Bind();
            GL.Uniform1(location, data);
        }

        public void SetFloatArray(string name, float[] data)
        {
            Bind();
            GL.Uniform1(GetUniformLocation(name), data.Length, data);
        }

        public void SetFloatArray(int location, float[] data)
        {
            Bind();
            GL.Uniform1(location, data.Length, data);
        }

        public void SetVector2(string name, Vector2 data)
        {
            Bind();
            GL.Uniform2(GetUniformLocation(name), data);
        }

        public void SetVector2(int location, Vector2 data)
        {
            Bind();
            GL.Uniform2(location, data);
        }

        public void SetVector2i(string name, Vector2i data)
        {
            Bind();
            GL.Uniform2(GetUniformLocation(name), data);
        }

        public void SetVector2i(int location, Vector2i data)
        {
            Bind();
            GL.Uniform2(location, data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            Bind();
            GL.Uniform3(GetUniformLocation(name), data);
        }

        public void SetVector3(int location, Vector3 data)
        {
            Bind();
            GL.Uniform3(location, data);
        }

        public void SetVector4(string name, Vector4 data)
        {
            Bind();
            GL.Uniform4(GetUniformLocation(name), data);
        }

        public void SetVector4(int location, Vector4 data)
        {
            Bind();
            GL.Uniform4(location, data);
        }

        public void SetColor(string name, Color4 data)
        {
            Bind();
            GL.Uniform4(GetUniformLocation(name), data);
        }

        public void SetColor(int location, Color4 data)
        {
            Bind();
            GL.Uniform4(location, data);
        }

        public unsafe void SetVector2Array(string name, Vector2[] data)
        {
            Bind();
            fixed (Vector2* p = &data[0])
            {
                GL.Uniform2(GetUniformLocation(name), data.Length, (float*)p);
            }
        }

        public unsafe void SetVector2Array(int location, Vector2[] data)
        {
            Bind();
            fixed (Vector2* p = &data[0])
            {
                GL.Uniform2(location, data.Length, (float*)p);
            }
        }

        public unsafe void SetVector2iArray(string name, Vector2i[] data)
        {
            Bind();
            fixed (Vector2i* p = &data[0])
            {
                GL.Uniform2(GetUniformLocation(name), data.Length, (int*)p);
            }
        }

        public unsafe void SetVector2iArray(int location, Vector2i[] data)
        {
            Bind();
            fixed (Vector2i* p = &data[0])
            {
                GL.Uniform2(location, data.Length, (int*)p);
            }
        }

        public unsafe void SetVector3Array(string name, Vector3[] data)
        {
            Bind();
            fixed (Vector3* p = &data[0])
            {
                GL.Uniform3(GetUniformLocation(name), data.Length, (float*)p);
            }
        }

        public unsafe void SetVector3Array(int location, Vector3[] data)
        {
            Bind();
            fixed (Vector3* p = &data[0])
            {
                GL.Uniform3(location, data.Length, (float*)p);
            }
        }

        public unsafe void SetVector4Array(string name, Vector4[] data)
        {
            Bind();
            fixed (Vector4* p = &data[0])
            {
                GL.Uniform4(GetUniformLocation(name), data.Length, (float*)p);
            }
        }

        public unsafe void SetVector4Array(int location, Vector4[] data)
        {
            Bind();
            fixed (Vector4* p = &data[0])
            {
                GL.Uniform4(location, data.Length, (float*)p);
            }
        }

        public unsafe void SetColorArray(string name, Color4[] data)
        {
            Bind();
            fixed (Color4* p = &data[0])
            {
                GL.Uniform4(GetUniformLocation(name), data.Length, (float*)p);
            }
        }

        public unsafe void SetColorArray(int location, Color4[] data)
        {
            Bind();
            fixed (Color4* p = &data[0])
            {
                GL.Uniform4(location, data.Length, (float*)p);
            }
        }

        public void SetMatrix3(string name, Matrix3 data)
        {
            Bind();
            GL.UniformMatrix3(GetUniformLocation(name), true, ref data);
        }

        public void SetMatrix3(int location, Matrix3 data)
        {
            Bind();
            GL.UniformMatrix3(location, true, ref data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            Bind();
            GL.UniformMatrix4(GetUniformLocation(name), true, ref data);
        }

        public void SetMatrix4(int location, Matrix4 data)
        {
            Bind();
            GL.UniformMatrix4(location, true, ref data);
        }

        public void SetMatrix4x2(string name, Matrix4x2 data)
        {
            Bind();
            GL.UniformMatrix4x2(GetUniformLocation(name), true, ref data);
        }

        public void SetMatrix4x2(int location, Matrix4x2 data)
        {
            Bind();
            GL.UniformMatrix4x2(location, true, ref data);
        }

        public void SetMatrix4x3(string name, Matrix4x3 data)
        {
            Bind();
            GL.UniformMatrix4x3(GetUniformLocation(name), true, ref data);
        }

        public void SetMatrix4x3(int location, Matrix4x3 data)
        {
            Bind();
            GL.UniformMatrix4x3(location, true, ref data);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteProgram(Handle);
        }

        protected static int CompileShader(string path, ShaderType shaderType)
        {
            var shaderSource = File.ReadAllText(path);
            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, shaderSource);
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Shader compilation error.\n\n{infoLog}");
            }
            return shader;
        }

        protected static void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                var infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Error linking Shader Program.\n\n{infoLog}");
            }
        }
    }
}
