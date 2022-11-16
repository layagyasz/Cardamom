﻿using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui
{
    public class UiContext
    {
        private readonly MouseListener _mouseListener;

        private IUiInteractiveElement? _topElement;

        public UiContext(MouseListener mouseListener)
        {
            _mouseListener = mouseListener;
        }

        public void Clear()
        {
            _topElement = null;
        }

        public IUiInteractiveElement? GetTopElement()
        {
            return _topElement;
        }

        public void Register(IUiInteractiveElement element, Transform transform)
        {
            if (element.IsPointWithinBounds(transform.GetInverse() * _mouseListener.GetMousePosition()))
            {
                _topElement = element;
            }
        }
    }
}
