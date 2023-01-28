using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing
{
    public class Canvas : IDisposable
    {
        public int Id { get; }
        private readonly Texture _texture;

        public Canvas(int id, Vector2i size, Color4 color)
        {
            Id = id;
            _texture = Texture.Create(size, color);
        }

        public Canvas(Texture texture)
        {
            _texture = texture;
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

        public override string ToString()
        {
            return string.Format($"[Canvas: Id={Id}]");
        }
    }
}
