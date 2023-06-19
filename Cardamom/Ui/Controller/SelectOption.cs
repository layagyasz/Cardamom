namespace Cardamom.Ui.Controller
{
    public class SelectOption<T>
    {
        public T Value { get; }
        public string Text { get; }

        private SelectOption(T value, string text)
        {
            Value = value;
            Text = text;
        }

        public static SelectOption<T> Create(T value, string text)
        {
            return new(value, text);
        }
    }
}
