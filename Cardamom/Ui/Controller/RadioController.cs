using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements;
using OpenTK.Graphics.GL;

namespace Cardamom.Ui.Controller
{
    public class RadioController<T> : IController, IFormElementController<string, T>
    {
        public EventHandler<ValueChangedEventArgs<string, T?>>? ValueChanged { get; set; }

        public string Key { get; }

        UiCompoundComponent? _element;
        private IOptionController<T>? _selected;
        private T? _value;

        public RadioController(string key)
        {
            Key = key;
        }

        public T? GetValue()
        {
            return _value;
        }

        public void SetValue(T? value)
        {
            if (value == null)
            {
                SetSelected(null);
                return;
            }
            foreach (var element in _element!.Cast<IUiElement>())
            {
                if (element.Controller is IOptionController<T> controller)
                {
                    if (Equals(controller.Key, value))
                    {
                        SetSelected(controller);
                        return;
                    }
                }
            }
            throw new ArgumentException("No Option found for value.");
        }

        public void Bind(object @object)
        {
            _element = (UiCompoundComponent)@object;
            _element.ElementAdded += HandleElementAdded;
            _element.ElementRemoved += HandleElementRemoved;
            foreach (var option in _element.Cast<IUiElement>())
            {
                BindElement(option);
            }
            var selected = 
                _element!
                    .SelectMany(GetControllers)
                    .Where(x => x is IOptionController<T>)
                    .Cast<IOptionController<T>>()
                    .FirstOrDefault();
            if (selected != null)
            {
                SetSelected(selected);
            }
        }

        public void Unbind()
        {
            foreach (var option in _element!.Cast<IUiElement>())
            {
                UnbindElement(option);
            }
            _element!.ElementAdded -= HandleElementAdded;
            _element!.ElementRemoved -= HandleElementRemoved;
            _element = null;
        }

        private void BindElement(IUiElement element)
        {
            if (element.Controller is IOptionController<T> controller)
            {
                controller.Selected += HandleElementSelected;
            }
            if (element is UiCompoundComponent compound 
                && compound.ComponentController is IOptionController<T> componentController)
            {
                componentController.Selected += HandleElementSelected;
            }
        }

        private void UnbindElement(IUiElement element)
        {
            if (element.Controller is IOptionController<T> controller)
            {
                controller.Selected -= HandleElementSelected;
            }
            if (element is UiCompoundComponent compound
                && compound.ComponentController is IOptionController<T> componentController)
            {
                componentController.Selected -= HandleElementSelected;
            }
        }

        private void SetSelected(IOptionController<T>? elementController)
        {
            _selected?.SetSelected(false);
            if (elementController == null)
            {
                _selected = null;
                _value = default;
                ValueChanged?.Invoke(this, new(Key, _value));
            }
            else if (elementController is IOptionController<T> controller)
            {
                controller.SetSelected(true);
                _selected = controller;
                _value = controller.Key;
                ValueChanged?.Invoke(this, new(Key, _value));
            }
        }

        private void HandleElementAdded(object? sender, ElementEventArgs e)
        {
            BindElement((IUiElement)e.Element);
        }

        private void HandleElementSelected(object? sender, EventArgs e)
        {
            SetSelected((IOptionController<T>)sender!);
        }

        private void HandleElementRemoved(object? sender, ElementEventArgs e)
        {
            UnbindElement((IUiElement)e.Element);
        }
        
        private static IEnumerable<IController> GetControllers(IUiElement element)
        {
            yield return element.Controller;
            if (element is UiCompoundComponent compound)
            {
                yield return compound.ComponentController;
            }
        }
    }
}
