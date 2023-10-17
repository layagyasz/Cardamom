namespace Cardamom.Ui.Controller
{
    public class NoOpController : IController
    {
        protected object? _element;

        public void Bind(object @object)
        {
            _element = @object;
        }

        public void Unbind()
        {
            _element = default;
        }
    }
}
