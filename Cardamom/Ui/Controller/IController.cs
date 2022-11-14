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
        void Bind(object @object);
        void Unbind();
        void HandleKeyPressed(KeyEventArgs e);
        void HandleTextEntered(TextEventArgs e);
        void HandleMouseEntered();
        void HandleMouseLeft();
        void HandleMouseButtonClicked(MouseButtonEventArgs e);
        void HandleMouseButtonDragged(MouseButtonDragEventArgs e);
        void HandleMouseWheelScrolled(MouseWheelScrollEventArgs e);
        void HandleFocusEntered();
        void HandleFocusLeft();
    }
}
