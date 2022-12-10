using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing
{
    public class Canvas : IDisposable
    {
        private readonly Texture _texture;

        public Canvas(Vector2i size, Color4 color)
        {
            _texture = Texture.Create(size, color);
        }

        public Texture GetTexture()
        {
            return _texture;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
            _texture.Dispose();
        }
    }
}
