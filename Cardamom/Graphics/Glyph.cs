using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public struct Glyph
    {
        public float Advance { get; set; }
        public Box2 Bounds { get; set; }
        public Box2i TextureView { get; set; }
        public float LeftBuffer { get; set; }
        public float RightBuffer { get; set; }
    }
}
