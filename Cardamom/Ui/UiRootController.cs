using Cardamom.Ui.Controller;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardamom.Ui
{
    public class UiRootController
    {
        private readonly MouseListener _mouseListener;

        private IControlled? _focus;
        private IControlled? _mouseOver;

        public UiRootController(RenderWindow window, MouseListener mouseListener)
        {
            window.KeyPressed += HandleKeyPressed;
            window.TextEntered += HandleTextEntered;

            _mouseListener = mouseListener;
            _mouseListener.MouseButtonClicked += HandleMouseButtonClicked;
        }

        private void HandleKeyPressed(object? sender, KeyEventArgs e)
        {
            _focus?.Controller?.HandleKeyPressed(e);
        }

        private void HandleTextEntered(object? sender, TextEventArgs e)
        {
            _focus?.Controller?.HandleTextEntered(e);
        }

        private void HandleMouseButtonClicked(object? sender, MouseButtonEventArgs e)
        {
            // Translate into component relative coordinates.
            _mouseOver?.Controller?.HandleMouseButtonClicked(e);
        }
    }
}
