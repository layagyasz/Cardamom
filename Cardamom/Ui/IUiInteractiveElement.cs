using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom.Ui
{
    public interface IUiInteractiveElement : IUiElement
    {
        bool IsPointWithinBounds(Vector2f point);
    }
}
