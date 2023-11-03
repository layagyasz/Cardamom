using OpenTK.Mathematics;

namespace Cardamom.Graphics.TexturePacking
{
    public record class TextureSegment(string Key, Texture? Texture, Box2i TextureView) : IKeyed
    {
        public string Key { get; set; } = Key;
    }
}
