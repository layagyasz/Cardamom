using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Shader : GLObject
    {

        private readonly Dictionary<string, int> _uniformLocations;

        public Shader(string vertexShaderPath, string fragmentShaderPath) 
            : base(GL.CreateProgram())
        {
            var vertexShader = CompileShader(vertexShaderPath, ShaderType.VertexShader);
            var fragmentShader = CompileShader(fragmentShaderPath, ShaderType.FragmentShader);

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                var location = GL.GetUniformLocation(Handle, key);
                _uniformLocations.Add(key, location);
            }
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
                throw new Exception("Error occurred whilst linking Shader Program");
            }
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
    }
}
