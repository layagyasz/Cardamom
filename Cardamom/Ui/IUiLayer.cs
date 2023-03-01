using Cardamom.Graphics;
using Cardamom.Ui.Controller;

namespace Cardamom.Ui
{
    public interface IUiLayer : IRenderable 
    { 
        IController Controller { get; }
    }
}