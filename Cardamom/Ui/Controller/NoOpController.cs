namespace Cardamom.Ui.Controller
{
    public class NoOpController<T> : IController
    {
        protected T? _element;

        public void Bind(object @object)
        {
            _element = (T)@object;
        }

        public void Unbind()
        {
            _element = default;
        }
    }
}
