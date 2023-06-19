using Cardamom.Graphics;
using Cardamom.Ui.Controller.Element;

namespace Cardamom.Ui.Elements
{
    public class Select : TextUiElement, IUiContainer
    {
        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public int Count => _dropBox.Count;

        private readonly UiSerialContainer _dropBox;
        private bool _open;

        public Select(Class @class, IElementController controller, UiSerialContainer dropBox)
            : base(@class, controller, string.Empty)
        {
            _dropBox = dropBox;
            _dropBox.Parent = this;
            _dropBox.ElementAdded += HandleOptionAdded;
            _dropBox.ElementRemoved += HandleOptionRemoved;
        }

        public void Add(IUiElement element)
        {
            _dropBox.Add(element);
        }

        public void Clear(bool dispose)
        {
            _dropBox.Clear(dispose);
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _dropBox.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Initialize()
        {
            _dropBox.Initialize();
            base.Initialize();
        }

        public void Insert(int index, IUiElement element)
        {
            _dropBox.Insert(index, element);
        }

        public void Remove(IUiElement element)
        {
            _dropBox.Remove(element);
        }

        public void SetOpen(bool value)
        {
            _open = value;
        }

        public void ToggleOpen()
        {
            _open = !_open;
        }

        public override void Draw(IRenderTarget target, IUiContext context)
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

        private void HandleOptionAdded(object? sender, ElementEventArgs e)
        {
            ElementAdded?.Invoke(this, e);
        }

        private void HandleOptionRemoved(object? sender, ElementEventArgs e)
        {
            ElementRemoved?.Invoke(this, e);
        }
    }
}
