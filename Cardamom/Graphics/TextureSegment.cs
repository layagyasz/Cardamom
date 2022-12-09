namespace Cardamom.Graphics
{
    public class TextureSegment : IKeyed
    {
        public string? Key { get; set; }
        public Texture? Texture { get; set; }
        public IntRect TextureView { get; set; }

        public TextureSegment(string key, Texture? texture, IntRect textureView)
        {
            Key = key;
            Texture = texture;
            TextureView = textureView;
        }
    }
}
