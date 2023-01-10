using OpenTK.Mathematics;

namespace Cardamom.Graphics.TexturePacking
{
    public class TextureSegment : IKeyed
    {
        public string Key { get; set; }
        public Texture? Texture { get; set; }
        public Box2i TextureView { get; set; }

        public TextureSegment(string key, Texture? texture, Box2i textureView)
        {
            Key = key;
            Texture = texture;
            TextureView = textureView;
        }
    }
}
