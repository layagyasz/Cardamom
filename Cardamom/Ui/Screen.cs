﻿using Cardamom.Graphics;
using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public class Screen : IRenderable
    {
        public IController Controller { get; }

        protected readonly IUiLayer[] _uiLayers;

        public Screen(IController controller, IEnumerable<IUiLayer> uiLayers)
        {
            Controller = controller;
            _uiLayers = uiLayers.ToArray();
        }

        public virtual void Initialize()
        {
            Controller.Bind(this);
            foreach (var layer in _uiLayers)
            {
                layer.Initialize();
            }
        }

        public virtual void ResizeContext(Vector3 bounds) { }

        public virtual void Draw(RenderTarget target, UiContext context)
        {
            foreach (var layer in _uiLayers)
            {
                layer.Draw(target, context);
            }
        }

        public virtual void Update(long delta)
        {
            foreach (var layer in _uiLayers)
            {
                layer.Update(delta);
            }
        }
    }
}
