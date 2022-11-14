using Cardamom.Ui.Controller;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom.Ui
{
    public interface IUiElement : IRenderable, IControlled
    {
        Vector2f Position { get; set; }
        Vector2f Size { get; }
    }
}
