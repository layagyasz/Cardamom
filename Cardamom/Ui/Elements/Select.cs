using Cardamom.Ui.Controller;
using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class Select : UiRootComponent
    {
        public class Style
        {
            public string? Root { get; set; }
            public string? OptionContainer { get; set; }
            public string? Option { get; set; }
        }

        public TextUiElement Root { get; }
        public UiCompoundComponent Options { get; }

        private readonly Class _optionClass;

        public Select(IController controller, TextUiElement root, UiCompoundComponent options, Class optionClass)
            : base(controller, root)
        {
            Root = root;
            Options = options;
            Options.Position = root.LeftMargin + new Vector3(0, root.TrueSize.Y, 0);
            Options.Visible = false;
            _optionClass = optionClass;

            Add(options);
        }

        public void AddOption<T>(SelectOption<T> option)
        {
            var o = new TextUiElement(_optionClass, new OptionElementController<T>(option.Value), option.Text);
            o.Initialize();
            Options.Add(o);
        }

        public void Clear()
        {
            Options.Clear(/* dispose= */ true);
        }

        public void SetOpen(bool open)
        {
            Options.Visible = open;
        }
    }
}
