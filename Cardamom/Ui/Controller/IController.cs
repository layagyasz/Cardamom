namespace Cardamom.Ui.Controller
{
    public interface IController
    {
        void Bind(object @object);
        void Unbind();
    }
}
