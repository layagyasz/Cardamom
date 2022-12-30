using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public class RenderShader : GLShader
    {
        private RenderShader(int handle)
            : base(handle) { }

        public class Builder
        {
            public string? Vertex { get; set; }
            public string? Fragment { get; set; }
            public string? Geometry { get; set; }
            public string? TesselationControl { get; set; }
            public string? TesselationEvaluation { get; set; }

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

            public RenderShader Build()
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

                return new RenderShader(program);
            }
        }
    }
}
