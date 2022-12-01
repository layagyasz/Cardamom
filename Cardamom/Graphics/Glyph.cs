namespace Cardamom.Graphics
{
    public struct Glyph
    {
        public float Advance { get; set; }
        public FloatRect Bounds { get; set; }
        public IntRect TextureView { get; set; }
    }
}
