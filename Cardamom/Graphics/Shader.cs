using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Reflection.Metadata.Ecma335;

namespace Cardamom.Graphics
{
    public class Shader : GLObject
    {
        private readonly Dictionary<string, int> _uniformLocations;

        private Shader(int handle, Dictionary<string, int> uniformLocations) 
            : base(handle)
        {
            _uniformLocations = uniformLocations;
        }

        public void Bind()
        {
            GL.UseProgram(Handle);
            Error.LogGLError("bind shader");
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetInt32(string name, int data)
        {
            Bind();
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            Bind();
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void SetMatrix3(string name, Matrix3 data)
        {
            Bind();
            GL.UniformMatrix3(_uniformLocations[name], true, ref data);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            Bind();
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            Bind();
            GL.Uniform3(_uniformLocations[name], data);
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

                GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
                var uniformLocations = new Dictionary<string, int>();
                for (var i = 0; i < numberOfUniforms; i++)
                {
                    var key = GL.GetActiveUniform(program, i, out _, out _);
                    var location = GL.GetUniformLocation(program, key);
                    uniformLocations.Add(key, location);
                }

                return new Shader(program, uniformLocations);
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
                    throw new Exception("Error linking Shader Program");
                }
            }
        }
    }
}
