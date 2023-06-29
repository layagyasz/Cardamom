namespace Cardamom.Ui.Controller
{
    public interface IRandomizableFormFieldController<T> : IFormFieldController<T>
    {
        void Randomize(Random random, bool notify = true);
    }
}
