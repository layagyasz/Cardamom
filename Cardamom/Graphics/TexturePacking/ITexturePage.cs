using OpenTK.Mathematics;

namespace Cardamom.Graphics.TexturePacking
{
    public interface ITexturePage
    {
        Texture GetTexture();
        bool Add(Texture texture, out Box2i bounds);
        bool Add(Bitmap bitmap, out Box2i bounds);
    }
}
