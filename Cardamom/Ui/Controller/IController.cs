using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom.Ui.Controller
{
    public interface IController
    {
        void HandleKeyPressed(KeyEventArgs e);
        void HandleTextEntered(TextEventArgs e);
        void HandleMouseEntered();
        void HandleMouseLeft();
        void HandleMouseButtonClicked(MouseButtonEventArgs e);
        void HandleMouseWheelScrolled(MouseWheelScrollEventArgs e);
    }
}
