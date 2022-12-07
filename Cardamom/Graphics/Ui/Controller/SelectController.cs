using Cardamom.Graphics.Ui.Elements;

namespace Cardamom.Graphics.Ui.Controller
{
    public class SelectController<T> : ClassedUiElementController<Select>, IFormElementController<string, T>
    {
        public string Key { get; }

        private SelectOptionController<T>? _selected;
        private T? _value;

        public SelectController(string Key)
        {
            this.Key = Key;
        }

        public T? GetValue()
        {
            return _value;
        }

        public void SetValue(T? value)
        {
            _value = value;
        }

        public override void Bind(object @object)
        {
            base.Bind(@object);
            if (_element!.GetDropBox().Controller is TableController dropBoxController)
            {
                dropBoxController.ElementClicked = HandleElementClicked;
            }
            SetSelected(_element!.GetDropBox().First().Controller);
        }

        public override bool HandleMouseEntered()
        {
            SetHover(true);
            if (GetFocus())
            {
                _element!.SetOpen(true);
            }
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            _element!.SetOpen(false);
            return true;
        }

        public override bool HandleFocusEntered()
        {
            SetFocus(true);
            _element!.SetOpen(true);
            return true;
        }

        public override bool HandleFocusLeft()
        {
            SetFocus(false);
            _element!.SetOpen(false);
            return true;
        }

        private void SetSelected(IController elementController)
        {
            if (elementController is SelectOptionController<T> controller)
            {
                _selected?.SetValue(false);
                controller.SetValue(true);
                _element!.SetText(controller.GetText());
                _selected = controller;
                _value = controller.Key;
                Console.WriteLine(_value);
            }
            else
            {
                throw new InvalidOperationException("Selects may only contain SelectOptions.");
            }
        }

        private void HandleElementClicked(object? sender, ElementEventArgs e)
        {
            SetSelected(e.Element);
        }
    }
}
