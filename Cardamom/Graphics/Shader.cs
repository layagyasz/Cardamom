using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Xml.Linq;

namespace Cardamom.Graphics
{
    public class Shader : GLObject
    {
        private Shader(int handle)
            : base(handle) { }

        public void Bind()
        {
            GL.UseProgram(Handle);
            Error.LogGLError("bind shader");
        }

        public void DoCompute(Vector2i size)
        {
            Bind();
            GL.DispatchCompute(size.X, size.Y, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
        }

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(Handle, name);
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(Handle, name);
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

        protected override void DisposeImpl()
        {
            GL.DeleteProgram(Handle);
        }

        public class Builder
        {
            public string? Vertex { get; set; }
            public string? Fragment { get; set; }
            public string? Geometry { get; set; }
            public string? TesselationControl { get; set; }
            public string? TesselationEvaluation { get; set; }
            public string? Compute { get; set; }

            public Builder SetVertex(string path)
            {
                Vertex = path;
                return this;
            }

            public Builder SetFragment(string path)
            {
                Fragment = path;
                return this;
            }

            public Builder SetGeometry(string path)
            {
                Geometry = path;
                return this;
            }

            public Builder SetTesselationControl(string path)
            {
                TesselationControl = path;
                return this;
            }

            public Builder SetTesselationEvaluation(string path)
            {
                TesselationEvaluation = path;
                return this;
            }

            public Builder SetCompute(string path)
            {
                Compute = path;
                return this;
            }

            public Shader Build()
            {
                var handles = new List<int>();
                if (Vertex != null)
                {
                    handles.Add(CompileShader(Vertex, ShaderType.VertexShader));
                }
                if (Fragment != null)
                {
                    handles.Add(CompileShader(Fragment, ShaderType.FragmentShader));
                }
                if (Geometry != null)
                {
                    handles.Add(CompileShader(Geometry, ShaderType.GeometryShader));
                }
                if (TesselationControl != null)
                {
                    handles.Add(CompileShader(TesselationControl, ShaderType.TessControlShader));
                }
                if (TesselationEvaluation != null)
                {
                    handles.Add(CompileShader(TesselationEvaluation, ShaderType.TessEvaluationShader));
                }
                if (Compute != null)
                {
                    handles.Add(CompileShader(Compute, ShaderType.ComputeShader));
                }

                int program = GL.CreateProgram();
                foreach (var handle in handles)
                {
                    GL.AttachShader(program, handle);
                }
                LinkProgram(program);
                foreach (var handle in handles)
                {
                    GL.DetachShader(program, handle);
                    GL.DeleteShader(handle);
                }

                return new Shader(program);
            }

            private static int CompileShader(string path, ShaderType shaderType)
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

            private static void LinkProgram(int program)
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
}
