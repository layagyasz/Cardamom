namespace Cardamom.Graphics
{
    public struct RenderResources
    {
        public BlendMode BlendMode { get; set; } = BlendMode.Alpha;
        public RenderShader Shader { get; set; }
        public Texture? Texture0 { get; set; }
        public Texture? Texture1 { get; set; }
        public Texture? Texture2 { get; set; }
        public bool IsPretransformed { get; set; }
        public bool EnableDepthTest { get; set; } = true;
        public bool EnableDepthMask { get; set; } = true;

        public RenderResources(BlendMode blendMode, RenderShader shader)
        {
            BlendMode = blendMode;
            Shader = shader;
        }

        public RenderResources(BlendMode blendMode, RenderShader shader, Texture texture0)
        {
            BlendMode = blendMode;
            Shader = shader;
            Texture0 = texture0;
        }

        public RenderResources(BlendMode blendMode, RenderShader shader, Texture texture0, Texture texture1)
        {
            BlendMode = blendMode;
            Shader = shader;
            Texture0 = texture0;
            Texture1 = texture1;
        }

        public RenderResources(
            BlendMode blendMode, RenderShader shader, Texture texture0, Texture texture1, Texture texture2)
        {
            BlendMode = blendMode;
            Shader = shader;
            Texture0 = texture0;
            Texture1 = texture1;
            Texture2 = texture2;
        }
    }
}
