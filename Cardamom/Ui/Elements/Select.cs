using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;

namespace Cardamom.Ui.Elements
{
    public class Select : TextUiElement
    {
        private readonly UiSerialContainer _dropBox;
        private bool _open;

        public Select(Class @class, IElementController controller, UiSerialContainer dropBox)
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

        public override void Draw(RenderTarget target, UiContext context)
        {
            base.Draw(target, context);
            if (_open)
            {
                target.PushEmptyScissor();
                _dropBox.Position = new(0, TrueSize.Y, Position.Z + 1);
                _dropBox.Draw(target, context);
                target.PopScissor();
            }
        }

        public override void Update(long delta)
        {
            base.Update(delta);
            if (_open)
            {
                _dropBox.Update(delta);
            }
        }
    }
}
