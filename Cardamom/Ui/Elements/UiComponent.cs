using Cardamom.Graphics;
using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiComponent : IUiGroup
    {
        public IController Controller { get; }

        private IUiGroup _group;

        public UiComponent(IController controller, IUiGroup group)
        {
            Controller = controller;
            _group = group;
        }

        public void Initialize()
        {
            Controller.Bind(this);
            _group.Initialize();
        }

        public void Draw(RenderTarget target, UiContext context)
        {
            _group.Draw(target, context);
        }

        public IEnumerator<IRenderable> GetEnumerator()
        {
            return _group.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ResizeContext(Vector3 bounds)
        {
            _group.ResizeContext(bounds);
        }

        public void Update(long delta)
        {
            _group.Update(delta);
        }
    }
}
