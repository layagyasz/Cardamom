using Cardamom.Graphics;
using Cardamom.Graphics.Camera;

namespace Cardamom.Ui
{
    public interface IScene : IRenderable, IInteractive
    {
        public ICamera Camera { get; }
    }
}
