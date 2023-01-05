namespace Cardamom.Graphics
{
    public struct RenderResources
    {
        public RenderShader Shader { get; set; }
        public Texture? Texture0 { get; set; }
        public Texture? Texture1 { get; set; }

        public RenderResources(RenderShader shader)
        {
            Shader = shader;
        }

        public RenderResources(RenderShader shader, Texture texture0)
        {
            Shader = shader;
            Texture0 = texture0;
        }

        public RenderResources(RenderShader shader, Texture texture0, Texture texture1)
        {
            Shader = shader;
            Texture0 = texture0;
            Texture1 = texture1;
        }
    }
}
