namespace Cardamom.Graphics.Ui.Controller
{
    public interface IControlled
    {
        IController Controller { get; }
        IControlled? Parent { get; set; }
    }
}
