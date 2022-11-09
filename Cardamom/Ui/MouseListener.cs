using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom.Ui
{
    public class MouseListener
    {
        public EventHandler<MouseButtonEventArgs>? MouseButtonClicked { get; set; }

        private RenderWindow _window;

        private Mouse.Button? _depressedButton;

        public MouseListener(RenderWindow window)
        {
            _window = window;
            window.MouseButtonPressed += HandleMouseButtonPressed;
            window.MouseButtonReleased += HandleMouseButtonReleased;
        }

        public Vector2i GetMousePosition()
        {
            return Mouse.GetPosition(_window);
        }

        private void HandleMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
            _depressedButton ??= e.Button;
        }

        private void HandleMouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (_depressedButton == e.Button)
            {
                _depressedButton = null;
                MouseButtonClicked?.Invoke(this, e);
            }
        }
    }
}
