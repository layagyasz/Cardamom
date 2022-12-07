using Cardamom.Graphics.Ui.Controller;

namespace Cardamom.Graphics.Ui.Elements
{
    public class Select : TextUiElement
    {
        private readonly UiSerialContainer _dropBox;
        private bool _open;

        public Select(Class @class, IController controller, UiSerialContainer dropBox)
            : base(@class, controller) 
        {
            _dropBox = dropBox;
            _dropBox.Parent = this;
        }

        public override void Initialize()
        {
            _dropBox.Initialize();
            base.Initialize();
        }

        public UiSerialContainer GetDropBox()
        {
            return _dropBox;
        }

        public void SetOpen(bool value)
        {
            _open = value;
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);
            if (_open)
            {
                target.PushEmptyScissor();
                _dropBox.Draw(target);
                target.PopScissor();
            }
        }

        public override void Update(UiContext context, long delta)
        {
            base.Update(context, delta);
            if (_open)
            {
                context.PushEmptyScissor();
                _dropBox.Position = new(0, TrueSize.Y);
                _dropBox.Update(context, delta);
                context.PopScissor();
            }
        }
    }
}
